using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Main
{
    public class _ScriptMainComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
