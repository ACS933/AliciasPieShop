using AliciasPieShop.Models;
using AliciasPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AliciasPieShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartController(IPieRepository pieRepository, IShoppingCart shoppingCart)
        {
            _pieRepository = pieRepository;
            _shoppingCart = shoppingCart;
        }

        public ViewResult Index()
        {
            // this method calls getShoppingCartItems() in order to get a list of the items in the users cart
            var items = _shoppingCart.GetShoppingCartItems();
            // assign the retrieved items to the correct property of the scoped instance of IShoppingCart
            _shoppingCart.ShoppingCartItems = items;

            // instantiate our ViewModel using parameters retrieved from our up-to-date IShoppingCart instance and return it
            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());
            return View(shoppingCartViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int pieId)
        {
            var selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);
            if (selectedPie != null)
            {
                _shoppingCart.AddToCart(selectedPie);
            }
            // RedirectToAction calls the Index action method when this method returns
            return(RedirectToAction("Index"));
        }

        public RedirectToActionResult RemoveFromShoppingCart(int pieId)
        {
            var selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);
            if (selectedPie != null)
            {
                _shoppingCart.RemoveFromCart(selectedPie);
            }
            return (RedirectToAction("Index"));
        }


    }

    
}
