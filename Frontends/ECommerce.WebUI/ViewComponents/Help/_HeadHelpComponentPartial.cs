using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Help
{
    public class _HeadHelpComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
