using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.AdminViewComponents
{
    public class _HeadAdminComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
