using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Trends
{
    public class _FilterTrendComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
