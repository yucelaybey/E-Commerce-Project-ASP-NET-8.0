using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Favorites
{
    public class _NotFoundFavoritesComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
