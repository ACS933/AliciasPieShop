namespace AliciasPieShop.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AliciasPieShopDbContext _aliciasPieShopDbContext;
        private readonly IShoppingCart _shoppingCart;

        public OrderRepository(AliciasPieShopDbContext aliciasPieShopDbContext, IShoppingCart shoppingCart)
        {
            _aliciasPieShopDbContext = aliciasPieShopDbContext;
            _shoppingCart = shoppingCart;
        }

        public void CreateOrder(Order order)  // when we receive an order, add it to the database
        {
            order.OrderPlaced = DateTime.Now;

            // get shopping cart from repository and add up the price of the stuff in it
            List<ShoppingCartItem>? shoppingCartItems = _shoppingCart.ShoppingCartItems;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            // initialise OrderDetails property of Order.cs
            order.OrderDetails = new List<OrderDetail>();

            // populate OrderDetails list with an OrderDetail object for each ShoppingCartItem in the List we got from the _shoppingCart repository
            foreach (ShoppingCartItem? shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Amount = shoppingCartItem.Amount,
                    PieId = shoppingCartItem.Pie.PieId,
                    Price = shoppingCartItem.Pie.Price
                };

                order.OrderDetails.Add(orderDetail);
            }

            _aliciasPieShopDbContext.Orders.Add(order);
            _aliciasPieShopDbContext.SaveChanges();
        }
    }
}
