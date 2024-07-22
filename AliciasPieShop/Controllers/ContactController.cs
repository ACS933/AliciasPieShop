using Microsoft.AspNetCore.Mvc;

namespace AliciasPieShop.Controllers
{
    public class ContactController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
