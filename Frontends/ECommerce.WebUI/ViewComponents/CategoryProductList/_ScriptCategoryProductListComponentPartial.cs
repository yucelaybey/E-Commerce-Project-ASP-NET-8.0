using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.CategoryProductList
{
    public class _ScriptCategoryProductListComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
