using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.Email
{
    public class _SuccessMessageComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
