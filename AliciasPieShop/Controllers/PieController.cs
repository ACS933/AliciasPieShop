using AliciasPieShop.Models;
using AliciasPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AliciasPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _pieRepository = pieRepository;
        }

        //public IActionResult List() 
        //{
        //    //ViewBag.CurrentCategory = "Cheese cakes";
        //    //return View(_pieRepository.AllPies);
        //    PieListViewModel pieListViewModel = new PieListViewModel(_pieRepository.AllPies, "All Pies:");
        //    return View(pieListViewModel);
        //}

        public ViewResult List(string category) 
        {
            IEnumerable<Pie> pies;
            string? currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies;
                currentCategory = "All Pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category).OrderBy(p => p.PieId);

                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;

            }
            return View(new PieListViewModel(pies, currentCategory));
        }

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
            {
                return NotFound();
            }
            return View(pie);
        }
    }
}
