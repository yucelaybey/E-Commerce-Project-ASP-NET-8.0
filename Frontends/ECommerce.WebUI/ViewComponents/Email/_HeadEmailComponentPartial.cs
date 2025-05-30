using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Email
{
    public class _HeadEmailComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
