using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryDtos;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.Main
{
    public class _HeaderMainComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _HeaderMainComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var claimsPrincipal = User as ClaimsPrincipal;

                // Token'dan email bilgisini al (cookie tabanlı kimlik doğrulaması için)
                var email = claimsPrincipal.Claims
                    .FirstOrDefault(x => x.Type == "Email")?.Value;

                // Token'dan roles bilgisini al (cookie tabanlı kimlik doğrulaması için)
                var roles = claimsPrincipal.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToList();

                var client = _httpClientFactory.CreateClient();

                // Kategorileri getir
                var categoryResponse = await client.GetAsync("https://localhost:7026/api/Categories");

                // Eğer email bilgisi yoksa boş bir liste gönder
                if (string.IsNullOrEmpty(email))
                {
                    // Kategorileri al
                    var categoryJsonData = await categoryResponse.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<GetCategoryDto>>(categoryJsonData);

                    var model = new GetCategoryAndUserInfoModel
                    {
                        Categories = categories,
                        UserInfo = null
                    };
                   
                    return View(model);
                }

                // Kullanıcı bilgilerini getir
                var userResponse = await client.GetAsync($"https://localhost:7026/api/Login/{email}");

                // API çağrıları başarılıysa
                if (categoryResponse.IsSuccessStatusCode && userResponse.IsSuccessStatusCode)
                {
                    // Kategorileri al
                    var categoryJsonData = await categoryResponse.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<GetCategoryDto>>(categoryJsonData);

                    // Kullanıcı bilgilerini al
                    var userJsonData = await userResponse.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfoDto>(userJsonData);

                    // Rolleri UserInfoDto'ya ekle
                    userInfo.Roles = roles;

                    var model = new GetCategoryAndUserInfoModel
                    {
                        Categories = categories,
                        UserInfo = userInfo
                    };

                    // Eğer veri yoksa boş bir liste ve null kullanıcı bilgisi gönder
                    if (categories == null || !categories.Any() || userInfo == null)
                    {
                        return View(model);
                    }

                    // Kategoriler ve kullanıcı bilgilerini View'e gönder
                    return View(model);
                }
                else
                {
                    var model = new GetCategoryAndUserInfoModel
                    {
                        Categories = null,
                        UserInfo = null
                    };
                    // API çağrıları başarısızsa boş bir liste ve null kullanıcı bilgisi gönder
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Hata loglama (örneğin, bir loglama kütüphanesi kullanabilirsin)
                Console.WriteLine($"Hata: {ex.Message}");

                var model = new GetCategoryAndUserInfoModel
                {
                    Categories = null,
                    UserInfo = null
                };

                // Hata durumunda boş bir liste ve null kullanıcı bilgisi gönder
                return View(model);
            }
        }
    }
}