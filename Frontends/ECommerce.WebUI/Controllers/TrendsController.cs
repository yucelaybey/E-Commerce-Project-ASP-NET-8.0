using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("Anasayfa")]
    public class TrendsController : Controller
    {
        [HttpGet("OneCikanlar")]
        public IActionResult OneCikanlar()
        {
            return View();
        }
    }
}
