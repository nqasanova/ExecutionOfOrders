using DemoApplication.Areas.Client.ViewModels.Account;
using DemoApplication.Contracts.Order;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;

        public AccountController(DataContext dataContext, IUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
        }

        [HttpGet("dashboard", Name = "client-account-dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("orders", Name = "client-account-orders")]
        public async Task<IActionResult> OrdersAsync()
        {
            var model = await _dataContext.Orders
                 .Select(b => new OrderViewModel(b.Id, StatusStatusCode.GetStatusCode((OrderStatus)b.Status), b.TotalPrice, b.CreatedAt))
                 .ToListAsync();

            return View(model);
        }
    }
}