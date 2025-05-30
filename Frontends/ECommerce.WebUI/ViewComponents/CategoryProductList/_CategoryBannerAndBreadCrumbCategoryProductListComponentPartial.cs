using ECommerce.Dto.CategoryDtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.ViewComponents.CategoryProductList
{
    public class _CategoryBannerAndBreadCrumbCategoryProductListComponentPartial : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _CategoryBannerAndBreadCrumbCategoryProductListComponentPartial(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            // API'den kategori bilgilerini çek
            var category = await _httpClient.GetFromJsonAsync<GetCategoryDto>($"https://localhost:7026/api/Categories/{categoryId}");
            if (category == null)
            {
                return Content("Kategori bulunamadı.");
            }

            // View'e kategori bilgilerini gönder
            return View(category);
        }
    }
}