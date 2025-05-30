using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Trends
{
    public class _BreadCrumbTrendComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
