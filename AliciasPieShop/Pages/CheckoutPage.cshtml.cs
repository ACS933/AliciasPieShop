using AliciasPieShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AliciasPieShop.Pages
{
    public class CheckoutPageModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private IShoppingCart _shoppingCart;

        // dependency injection and constructor injection done in exactly the normal way
        public CheckoutPageModel(IOrderRepository orderRepository, IShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }

        [BindProperty]
        public Order Order { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // this will look similar to the Post method in OrderController, since it is doing the same thing.
            // I believe the ONLY difference is that we return Pages instead of Views.


            // if the validation fails, return the checkout page so user can fill in form correctly

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "cart is empty - please add something to your cart before attempting to checkout");
            }

            // if custom validation passes, add order to order database set, clear cart, and redirect to checkout complete page. if not, return the checkout Page again
            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(Order);
                _shoppingCart.ClearCart();
                return RedirectToPage("CheckoutCompletePage");
            }

            return Page();
        }
    }
}
