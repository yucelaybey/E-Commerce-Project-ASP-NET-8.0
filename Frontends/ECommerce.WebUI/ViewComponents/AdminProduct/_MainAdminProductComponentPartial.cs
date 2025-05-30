using E_Commerce.Domain.Entities;
using ECommerce.Dto.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECommerce.WebUI.ViewComponents.AdminAddProduct
{
    public class _MainAdminProductComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _MainAdminProductComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Ürünleri API'den çek
                var productsResponse = await client.GetAsync("https://localhost:7026/api/Products");
                if (productsResponse.IsSuccessStatusCode)
                {
                    var productsJson = await productsResponse.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<GetProductDto>>(productsJson);

                    // Kategorileri API'den çek
                    var categoriesResponse = await client.GetAsync("https://localhost:7026/api/Categories");
                    if (categoriesResponse.IsSuccessStatusCode)
                    {
                        var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesJson);

                        // Her ürün için kategori adını bul
                        foreach (var product in products)
                        {
                            var category = categories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
                            if (category != null)
                            {
                                product.CategoryName = category.Name; // Kategori adını ata
                            }
                        }
                    }

                    return View("Default", products); // Ürün listesini gönder
                }
                else
                {
                    // API çağrısı başarısızsa boş bir liste gönder
                    return View("Default", new List<GetProductDto>());
                }
            }
            catch (Exception ex)
            {
                // Hata loglama
                Console.WriteLine($"Hata: {ex.Message}");

                // Hata durumunda boş bir liste gönder
                return View("Default", new List<GetProductDto>());
            }
        }
    }
}
