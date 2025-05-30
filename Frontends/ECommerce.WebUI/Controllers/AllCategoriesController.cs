using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("Anasayfa/Kategoriler")]
    public class AllCategoriesController : Controller
    {
        [HttpGet("")]
        public IActionResult AllCategories()
        {
            return View();
        }
    }
}
