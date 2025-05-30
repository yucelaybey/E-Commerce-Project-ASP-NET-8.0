using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("Anasayfa/Yardim")]
    public class HelpController : Controller
    {
        [Route("")]
        public IActionResult Help()
        {
            return View();
        }
    }
}
