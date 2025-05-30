using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.WarningModal
{
    public class _WarningModalComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
