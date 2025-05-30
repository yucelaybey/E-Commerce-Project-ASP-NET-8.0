using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Orders
{
    public class _NotOrderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
