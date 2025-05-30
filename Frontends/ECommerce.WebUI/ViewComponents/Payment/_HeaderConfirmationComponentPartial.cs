using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Payment
{
    public class _HeaderConfirmationComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
