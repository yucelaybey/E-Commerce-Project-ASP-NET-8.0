using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.ProductDetails
{
    public class _BreadCrumbProductDetailsComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(string categoryName, string productName)
        {
            ViewBag.CategoryName = categoryName;
            ViewBag.ProductName = productName;
            return View();
        }
    }
}