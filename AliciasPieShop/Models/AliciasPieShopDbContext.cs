using Microsoft.EntityFrameworkCore;

namespace AliciasPieShop.Models
{
    public class AliciasPieShopDbContext : DbContext
    {
        public AliciasPieShopDbContext(DbContextOptions<AliciasPieShopDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Pie> Pies { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }    
    }
}
