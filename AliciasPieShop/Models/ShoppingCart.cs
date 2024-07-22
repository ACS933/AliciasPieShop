using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AliciasPieShop.Models
{
    public class ShoppingCart: IShoppingCart
    {
        // we need to write concrete implementations of the attributes and methods of the IShoppingCart Interface to make this compile

        // add databse context, since the shopping cart needs to talk to our database
        private readonly AliciasPieShopDbContext _aliciasPieShopDbContext;

        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        private ShoppingCart(AliciasPieShopDbContext context)
        {
            _aliciasPieShopDbContext = context;
        }

        // static method which creates a new shopping cart instance
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            // we take the services collection as a parameter for this method, as this gives us access to sessions through DI.
            //sessions give us the ability to store and retain information about a returning user.
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;  // session either null or valid
            // 'session' here is information pertaining to a specific user, which is persisted through multiple usage sessions

            // ?? is the 'null coalescing operator - it returns the value to its left UNLESS that value is null,
            // in which case it executes the code to its right and returns that. It's quite similar to a ternary expression!
            AliciasPieShopDbContext context = services.GetService<AliciasPieShopDbContext>() ?? throw new Exception("error initialising");
            // here we use ?? to throw an error if services.GetService returns a null object when we try to get the DbContext

            // get cartId from session if it exists, or create a new GUID cart ID if no ID exists
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);  // 2 params of setString are key and new value for a string property of session

            // return a new shopping cart object with the DbContext as a parameter. also set the Id property to cartId
            return new ShoppingCart(context) { ShoppingCartId = cartId };
            
        }

        public void AddToCart(Pie pie)
        {
            // check if at least one of the selected pie is already in the user's cart
            var shoppingCartItem = _aliciasPieShopDbContext.ShoppingCartItems.SingleOrDefault(
                s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            // if the user does not already have this pie in their cart, create new ShoppingCart item and add it to database with amount=1
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId, // ShoppingCartId on the right is a property of ShoppingCart.cs
                    Pie = pie,
                    Amount = 1
                };
                _aliciasPieShopDbContext.Add(shoppingCartItem);
            }
            // if shoppingCartItem already exists for this user and pie, increase its amount by 1
            else
            {
                shoppingCartItem.Amount++;
            }

            _aliciasPieShopDbContext.SaveChanges();  // remember to save the database once we've finished modifying it1
        }

        public int RemoveFromCart(Pie pie)
        {
            // check if pie to be removed is in the cart, i.e if there is an item in the ShoppingCartItems DbSet with matching PieId and CartId 
            var shoppingCartItem = _aliciasPieShopDbContext.ShoppingCartItems.FirstOrDefault(
                s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);
            
            var localQuantityLeft = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    // if there is more than 1 of the pie in the cart, we decrement the shoppingCartItem's amount by 1 rather than removing
                    shoppingCartItem.Amount--;
                    localQuantityLeft = shoppingCartItem.Amount;
                }
                else
                {
                    // if the amount of the pie to be deleted = 1, remove the item from the database table
                    _aliciasPieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }

            }
            _aliciasPieShopDbContext.SaveChanges();
            return localQuantityLeft;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            // return the ShoppingCartItems property, unless it is null
            // if it is null, we set the property to the items in the shoppingCartItems table where the cart Id matches this.ShoppingCartId
            return ShoppingCartItems ??= _aliciasPieShopDbContext.ShoppingCartItems.Where(c =>
                c.ShoppingCartId == ShoppingCartId).Include(s => s.Pie).ToList();    
        }

        public void ClearCart()
        {
            // get all the items in the user's cart from the ShoppingCartItems table
            var allItemsInCart = _aliciasPieShopDbContext.ShoppingCartItems.Where(item => item.ShoppingCartId == ShoppingCartId);

            // delete all the items in the user's cart from the table. removerange lets us pass a IEnumerable as the param to delete
            _aliciasPieShopDbContext.ShoppingCartItems.RemoveRange(allItemsInCart);

            // save as always 
            _aliciasPieShopDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            // get the items in the users cart
            // sum the product of pie price and pie amount for each ShoppingCartItem in the users cart
            var total = _aliciasPieShopDbContext.ShoppingCartItems.Where(c =>
                c.ShoppingCartId == ShoppingCartId).Select(c => c.Pie.Price * c.Amount).Sum();

            return total;
        }




    }
}
