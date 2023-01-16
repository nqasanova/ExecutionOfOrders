using DemoApplication.Areas.Client.ViewModels.Checkout;
using DemoApplication.Contracts.Order;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using DemoApplication.Services.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DemoApplication.Areas.Client.Controllers
{

    [Area("client")]
    [Route("checkout")]
    public class CheckoutController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;


        public CheckoutController(DataContext dbContext, IUserService userService, IOrderService orderService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _orderService = orderService;
        }

        #region List

        [HttpGet("list", Name = "checkout-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = new ProductListItemViewModel
            {
                Products = await _dbContext.BasketProducts.Include(bp => bp.Book)
                .Select(bp => new ProductListItemViewModel.ListItem(bp.BookId, bp.Quantity, bp.Book.Title, bp.Book.Price, bp.Book.Price * bp.Quantity))
                .ToListAsync()
            };

            return View(model);
        }

        #endregion

        #region Order
        [HttpPost("place-order", Name = "client-checkout-place-order")]
        public async Task<IActionResult> PlaceOrder()
        {
            var basketProducts = _dbContext.BasketProducts.Include(bp => bp.Book).Select(bp => new
            ProductListItemViewModel.ListItem(bp.BookId, bp.Quantity, bp.Book.Title, bp.Book.Price, bp.Book.Price * bp.Quantity)).ToList();

            var createOrder = await CreateOrder();

            foreach (var basketProduct in basketProducts)
            {
                var orderProduct = new OrderedProduct
                {
                    BookId = basketProduct.Id,
                    Quantity = basketProduct.Quantity,
                    OrderId = createOrder.Id
                };

                _dbContext.OrderedProducts.Add(orderProduct);
            }

            await DeleteBasketProducts();
            _dbContext.SaveChanges();

            async Task<Order> CreateOrder()
            {
                var order = new Order
                {
                    Id = _orderService.OrderCode,
                    UserId = _userService.CurrentUser.Id,
                    Status = (int)OrderStatus.Created,
                    TotalPrice = _dbContext.BasketProducts.
                    Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).Sum(bp => bp.Book.Price * bp.Quantity)

                };

                await _dbContext.Orders.AddAsync(order);

                return order;
            }

            async Task DeleteBasketProducts()
            {
                var removedBasketProducts = await _dbContext.BasketProducts.Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).ToListAsync();

                removedBasketProducts.ForEach(bp => _dbContext.BasketProducts.Remove(bp));
            }
            return RedirectToRoute("checkout-list");
        }

        #endregion
    }
}