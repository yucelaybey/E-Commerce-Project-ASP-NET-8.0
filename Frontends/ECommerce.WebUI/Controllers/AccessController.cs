using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("Erisim")]
    public class AccessController : Controller
    {
        [Route("ErisimKisitlamasi")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
