using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Orders
{
    public class _HeadOrdersComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
