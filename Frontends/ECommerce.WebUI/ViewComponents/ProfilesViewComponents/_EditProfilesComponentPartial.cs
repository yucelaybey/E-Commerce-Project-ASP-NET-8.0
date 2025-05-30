using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.ProfilesViewComponents
{
    public class _EditProfilesComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
