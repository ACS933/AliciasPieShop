using Microsoft.EntityFrameworkCore;

namespace AliciasPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AliciasPieShopDbContext _aliciasPieShopDbContext;

        public PieRepository(AliciasPieShopDbContext aliciasPieShopDbContext)
        {
            _aliciasPieShopDbContext = aliciasPieShopDbContext;
        }

        // we need to implement all the functionality set out in IPieRepository
        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _aliciasPieShopDbContext.Pies.Include(c => c.Category);
            }
        }
        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _aliciasPieShopDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int pieId)
        {
            return _aliciasPieShopDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }
    }
}
