using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _HeaderPaymentComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
