using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.AdminProduct
{
    public class _ProductScriptComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
