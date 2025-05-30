using ECommerce.Dto.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECommerce.WebUI.ViewComponents.AdminAddCategory
{
    public class _MainAdminCategoryComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _MainAdminCategoryComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync("https://localhost:7026/api/Categories");

                // API çağrısı başarılıysa
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<GetCategoryDto>>(jsonData);

                    // Eğer veri yoksa boş bir liste gönder
                    if (values == null || !values.Any())
                    {
                        return View(new List<GetCategoryDto>());
                    }

                    return View(values);
                }
                else
                {
                    // API çağrısı başarısızsa boş bir liste gönder
                    return View(new List<GetCategoryDto>());
                }
            }
            catch (Exception ex)
            {
                // Hata loglama (örneğin, bir loglama kütüphanesi kullanabilirsin)
                Console.WriteLine($"Hata: {ex.Message}");

                // Hata durumunda boş bir liste gönder
                return View(new List<GetCategoryDto>());
            }
        }
    }
}
