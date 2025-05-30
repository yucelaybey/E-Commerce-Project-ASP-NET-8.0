using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _HeadShoppingPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
