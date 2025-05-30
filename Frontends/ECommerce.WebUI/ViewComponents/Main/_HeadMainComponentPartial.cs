using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Main
{
    public class _HeadMainComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
