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

        public IActionResult List() 
        {
            //ViewBag.CurrentCategory = "Cheese cakes";
            //return View(_pieRepository.AllPies);
            PieListViewModel pieListViewModel = new PieListViewModel(_pieRepository.AllPies, "Cheese Cakes");
            return View(pieListViewModel);
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
