using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Help
{
    public class _FAQHelpComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
