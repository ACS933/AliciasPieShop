using AliciasPieShop.Models;
using AliciasPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AliciasPieShop.Components
{
    public class ShoppingCartSummary: ViewComponent
    {
        // view components allow for DI and thus use of services, repositories and so on!
        private readonly IShoppingCart _shoppingCart;

        // constructor injection is also supported, obviously!
        public ShoppingCartSummary(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            return View(shoppingCartViewModel);
        }
    }
}
