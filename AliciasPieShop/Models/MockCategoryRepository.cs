namespace AliciasPieShop.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> AllCategories =>
            new List<Category>
            {
                new Category {CategoryId = 1, CategoryName = "Fruit pies", Description="sticky and messy!" },
                new Category {CategoryId = 2, CategoryName = "cheese cakes", Description="for when he's cheesed you off!"},
                new Category {CategoryId = 3, CategoryName = "seasonal pies", Description = "get in the holiday spirit with our seasonal pies! now with 20% extra whipped cream!"}
            };
    }
}
