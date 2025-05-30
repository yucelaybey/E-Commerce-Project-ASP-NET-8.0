using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Help
{
    public class _MainHelpComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
