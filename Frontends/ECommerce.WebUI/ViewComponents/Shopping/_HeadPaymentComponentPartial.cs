using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _HeadPaymentComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
