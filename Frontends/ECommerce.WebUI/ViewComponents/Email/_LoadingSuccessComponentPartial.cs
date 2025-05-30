using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Email
{
    public class _LoadingSuccessComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
