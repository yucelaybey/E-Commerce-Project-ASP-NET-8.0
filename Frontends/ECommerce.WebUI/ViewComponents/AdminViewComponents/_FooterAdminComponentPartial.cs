using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.AdminViewComponents
{
    public class _FooterAdminComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
