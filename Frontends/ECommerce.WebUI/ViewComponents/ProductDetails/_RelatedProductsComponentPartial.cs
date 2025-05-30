using ECommerce.Dto.CategoryProductList;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.ProductDetails
{
    public class _RelatedProductsComponentPartial : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _RelatedProductsComponentPartial(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            // API'den ilgili kategoriye ait ürünleri çek
            var relatedProducts = await _httpClient.GetFromJsonAsync<List<CategoryProductListDto>>($"https://localhost:7026/api/Products/by-category-with-cart-info/{categoryId}");

            // View'a ürün listesini gönder
            return View(relatedProducts);
        }
    }
}