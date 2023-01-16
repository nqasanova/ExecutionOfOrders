﻿using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using DemoApplication.Services.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public CartController(DataContext dataContext, IUserService userService, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
        }

        #region List

        [HttpGet("list", Name = "cart-book-list")]
        public async Task<IActionResult> ListAsync()
        {
            if (_userService.IsAuthenticated)
            {
                var model = await _dataContext.BasketProducts
                    .Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id)
                    .Select(bp =>
                        new ProductCookieViewModel(
                            bp.BookId,
                            bp.Book!.Title,
                            bp.Book.BookImages.Take(1).FirstOrDefault()! != null ? _fileService.GetFileUrl(bp.Book.BookImages!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Book) : String.Empty,
                            bp.Quantity,
                            bp.Book.Price,
                            bp.Book.Price * bp.Quantity))
                    .ToListAsync();

                return View(model);
            }

            var productsCookieValue = HttpContext.Request.Cookies["products"];
            var productsCookieViewModel = new List<ProductCookieViewModel>();

            if (productsCookieValue is not null)
            {
                productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productsCookieValue);
            }

            return View(productsCookieViewModel);
        }

        #endregion


        #region Delete

        [HttpGet("delete/{bookId}", Name = "cart-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int bookId)
        {
            if (_userService.IsAuthenticated)
            {

                var basketProduct = await _dataContext.BasketProducts
                        .FirstOrDefaultAsync(bp => bp.Basket.UserId == _userService.CurrentUser.Id && bp.BookId == bookId);

                if (basketProduct is null) return NotFound();

                _dataContext.BasketProducts.Remove(basketProduct);
            }

            else
            {
                var product = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
                if (product is null)
                {
                    return NotFound();
                }

                var productCookieValue = HttpContext.Request.Cookies["products"];
                if (productCookieValue is null)
                {
                    return NotFound();
                }

                var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                productsCookieViewModel!.RemoveAll(pcvm => pcvm.Id == bookId);

                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));
            }

            await _dataContext.SaveChangesAsync();
            return RedirectToRoute("cart-book-list");
        }
        #endregion
    }
}