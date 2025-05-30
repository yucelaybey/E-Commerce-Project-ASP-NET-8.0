using ECommerce.Dto.AppUserDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.AdminViewComponents
{
    public class _HeaderAdminComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _HeaderAdminComponentPartial(IHttpClientFactory httpClientFactory)
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

                // Eğer email bilgisi yoksa boş bir liste gönder
                if (string.IsNullOrEmpty(email))
                {
                    return View(null);
                }

                // Kullanıcı bilgilerini getir
                var userResponse = await client.GetAsync($"https://localhost:7026/api/Login/{email}");

                // API çağrıları başarılıysa
                if (userResponse.IsSuccessStatusCode)
                {
                    // Kullanıcı bilgilerini al
                    var userJsonData = await userResponse.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfoDto>(userJsonData);

                    // Rolleri UserInfoDto'ya ekle
                    userInfo.Roles = roles;

                    // Kategoriler ve kullanıcı bilgilerini View'e gönder
                    return View(userInfo);
                }
                else
                {
                    return View(null);
                }
            }
            catch (Exception ex)
            {
                // Hata loglama (örneğin, bir loglama kütüphanesi kullanabilirsin)
                Console.WriteLine($"Hata: {ex.Message}");

                // Hata durumunda boş bir liste ve null kullanıcı bilgisi gönder
                return View(null);
            }
        }
    }
}
