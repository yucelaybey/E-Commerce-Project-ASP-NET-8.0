using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Access
{
    public class _HeadAccessComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
