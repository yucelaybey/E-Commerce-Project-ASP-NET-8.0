using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Login
{
    public class _HeadLoginComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
