using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("Anasayfa", Order = 1)]
    public class MainController : Controller
    {
        [Route("")]
        public IActionResult Anasayfa()
        {
            return View();
        }
    }
}
