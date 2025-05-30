using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Access
{
    public class _ContactAccessComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
