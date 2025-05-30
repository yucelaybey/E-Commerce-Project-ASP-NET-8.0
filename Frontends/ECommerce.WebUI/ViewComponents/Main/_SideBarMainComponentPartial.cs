using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Main
{
    public class _SideBarMainComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
