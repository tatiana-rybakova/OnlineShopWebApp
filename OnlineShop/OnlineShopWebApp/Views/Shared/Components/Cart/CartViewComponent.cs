using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Views.Shared.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartsRepository cartsRepository;

        public CartViewComponent (ICartsRepository cartsRepository)
        {
            this.cartsRepository = cartsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await cartsRepository.TryGetByUserIdAsync(Constants.UserId);

            var cartViewModel = Mapping.ToCartViewModel(cart);
            
            var productsCount = cartViewModel?.Amount ?? 0;
            return View("Cart", productsCount); 
        }
    }
}
