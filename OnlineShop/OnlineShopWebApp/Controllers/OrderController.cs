using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartsRepository cartsRepository;
        private readonly IOrdersRepository ordersRepository;

        public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository)
        {
            this.cartsRepository = cartsRepository;
            this.ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyAsync(DeliveryInformationViewModel deliveryInformation)
        {
            if (ModelState.IsValid)
            {
                var cart = await cartsRepository.TryGetByUserIdAsync(Constants.UserId);
                ordersRepository.Add(cart.Items, Mapping.ToDbDelivery(deliveryInformation));
                await cartsRepository.ClearAsync(Constants.UserId);
                return View();
            }
            else
            {
                return View(deliveryInformation);
            }
        }
    }
}
