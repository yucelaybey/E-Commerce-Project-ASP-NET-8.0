using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.ProfilesViewComponents
{
    public class _HeadProfilesComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
