using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Main
{
    public class _FooterMainComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
