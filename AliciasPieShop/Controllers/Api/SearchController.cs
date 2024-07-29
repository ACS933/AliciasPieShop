using AliciasPieShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AliciasPieShop.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]                                         // this is an optional attribute, but it adds a few API-specific behaviours so you might want to include it
    public class SearchController : ControllerBase          // API controllers should inherit from ControllerBase, not Controller
    {
        private readonly IPieRepository _pieRepository;

        public SearchController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        [HttpGet]
        public IActionResult GetAll()                   // return all pies in JSON format
        {
            var allPies = _pieRepository.AllPies;
            return Ok(allPies);
        }

        [HttpGet("{id}")]                               // .NET Core can decide which get method to invoke based on whether the request includes an id
        public IActionResult GetByID(int id)
        {
            var pie = _pieRepository.GetPieById(id);

            if (pie == null)
            {
                return NotFound();
            }

            return Ok(pie);
        }

        [HttpPost]
        public IActionResult SearchPies([FromBody] string searchQuery)
        {
            IEnumerable<Pie> pies = new List<Pie>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                pies = _pieRepository.SearchPies(searchQuery);
            }
            // return data as JSON, since thats what is expected by our ajax call
            return new JsonResult(pies);
        }
    }
}
