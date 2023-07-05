using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class OrderController : Controller
    {
        private readonly IOrdersRepository ordersRepository;

        public OrderController(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        public async Task<IActionResult> OrdersAsync()
        {
            var orders = await ordersRepository.GetAllAsync();
            return View(orders.ToOrderViewModels());
        }

        public async Task<IActionResult> DetailsAsync(Guid orderId)
        {
            var order = await ordersRepository.TryGetByIdAsync(orderId);
            return View(order.ToOrderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatesync(Guid orderId, OrderStateViewModel state)
        {
            await ordersRepository.UpdateStateAsync(orderId, (OrderState)(int)state);
            return RedirectToAction("Orders");
        }
    }
}
