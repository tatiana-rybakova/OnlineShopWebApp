using Microsoft.AspNetCore.Mvc;
using System;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICartsRepository cartsRepository;

        public CartController(IProductsRepository productsRepository, ICartsRepository cartsRepository)
        {
            this.productsRepository = productsRepository;
            this.cartsRepository = cartsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await cartsRepository.TryGetByUserIdAsync(Constants.UserId);
            return View(Mapping.ToCartViewModel(cart));
        }
        public async Task<IActionResult> AddAsync(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
            await cartsRepository.AddAsync(product, Constants.UserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAsync(Guid itemId)
        {
            var product = productsRepository.TryGetById(itemId);
            await cartsRepository.DeleteAsync(product, Constants.UserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearAsync()
        {
            await cartsRepository.ClearAsync(Constants.UserId);
            return RedirectToAction("Index");
        }
    }
}
