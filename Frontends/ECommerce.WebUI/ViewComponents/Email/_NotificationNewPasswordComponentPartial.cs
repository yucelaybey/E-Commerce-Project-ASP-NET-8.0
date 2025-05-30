using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Email
{
    public class _NotificationNewPasswordComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
