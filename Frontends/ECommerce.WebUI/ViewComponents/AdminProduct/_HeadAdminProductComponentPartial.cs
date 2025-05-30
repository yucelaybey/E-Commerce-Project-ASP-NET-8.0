using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.AdminAddProduct
{
    public class _HeadAdminProductComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
