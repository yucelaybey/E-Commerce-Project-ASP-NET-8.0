using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Favorites
{
    public class _EmptyFavaritesComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
