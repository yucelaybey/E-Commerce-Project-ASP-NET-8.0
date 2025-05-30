using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Help
{
    public class _HeaderHelpComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
