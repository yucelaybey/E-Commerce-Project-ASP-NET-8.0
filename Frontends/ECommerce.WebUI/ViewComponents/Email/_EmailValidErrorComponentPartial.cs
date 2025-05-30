using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Email
{
    public class _EmailValidErrorComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
