using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.AdminOrder
{
    public class _OrderScriptComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
