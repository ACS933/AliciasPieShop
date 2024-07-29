using AliciasPieShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AliciasPieShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCart _shoppingCart;

        public OrderController(IShoppingCart shoppingCart, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }


        // [HttpGet] is assumed as default if http method is not specified
        public IActionResult Checkout()         // endpoint for when the user navigates to the checkout from the shopping cart  //GET
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            // the order here is created automatically by .NET Core using Model Binding! this is actually done BEFORE the action method is invoked!

            // make sure cart is up to date with dbContext
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            // check that the user has something in their cart before checkout (you can't buy 'nothing' after all)
            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                // ModelState is like a by-product of Model Binding.
                // while model binding is happening, any errors which occur are added to this ModelState
                // we can also add our own errors to the model state if we want
                ModelState.AddModelError("", "cart is empty - please add at least 1 pie before attempting to checkout.");
            }

            // if the model has no errors in it, either model binding errors or errors added conditionally by us
            if (ModelState.IsValid)
            {
                // if no errors, create order, add it to our database (Orders DbSet), empty the cart, and redirect to CheckoutComplete action method (which will return a checkoutComplete view)
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            
            // if there is an error, return the view with the order object as a parameter. this has the effect of not deleting all the entered text in the form after the user clicks submit.
            // this is because the view can re-populate its text fields with the information in the order object's properties.
            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for ordering some pies! delivery faster than Brett Lee guaranteed or your money back! ~ Alicia xo";   // viewbag, ewwww
            return View();
        }
    }
}
