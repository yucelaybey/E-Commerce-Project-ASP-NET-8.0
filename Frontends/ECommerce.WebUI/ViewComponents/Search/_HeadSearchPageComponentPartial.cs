using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Search
{
    public class _HeadSearchPageComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
