using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("/Anasayfa/Hesabim")]
    public class ProfilesController : Controller
    {
        [Route("")]
        public IActionResult Profiles()
        {
            return View();
        }
    }
}
