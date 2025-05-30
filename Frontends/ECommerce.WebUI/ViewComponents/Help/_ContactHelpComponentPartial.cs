using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Help
{
    public class _ContactHelpComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
