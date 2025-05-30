using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.CategoryProductList
{
    public class _FilterCategoryProductListComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
