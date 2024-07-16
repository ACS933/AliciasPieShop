namespace AliciasPieShop.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AliciasPieShopDbContext _aliciasPieShopDbContext;

        public CategoryRepository(AliciasPieShopDbContext aliciasPieShopDbContext)
        {
            _aliciasPieShopDbContext = aliciasPieShopDbContext;
        }

        public IEnumerable<Category> AllCategories
        {
            get
            {
                return _aliciasPieShopDbContext.Categories.OrderBy(c => c.CategoryName);
            }
        }
    }
}
