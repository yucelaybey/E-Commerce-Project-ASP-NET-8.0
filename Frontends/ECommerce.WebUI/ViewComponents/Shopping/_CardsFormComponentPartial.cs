using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _CardsFormComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
