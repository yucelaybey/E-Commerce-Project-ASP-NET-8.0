using ECommerce.Dto.AppUserDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _TopBarShoppingComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _TopBarShoppingComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var claimsPrincipal = User as ClaimsPrincipal;

                // Token'dan email bilgisini al
                var email = claimsPrincipal.Claims
                    .FirstOrDefault(x => x.Type == "Email")?.Value;

                // Token'dan roles bilgisini al
                var roles = claimsPrincipal.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToList();

                var client = _httpClientFactory.CreateClient();

                // Kullanıcı bilgilerini getir
                var userResponse = await client.GetAsync($"https://localhost:7026/api/Login/{email}");

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJsonData = await userResponse.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfoDto>(userJsonData);

                    // Rolleri ekle
                    userInfo.Roles = roles;

                    // Sadece userInfo'yu View'e gönder
                    return View(userInfo);
                }

                // Başarısız durumda null gönder
                return View((UserInfoDto)null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                // Hata durumunda null gönder
                return View((UserInfoDto)null);
            }
        }
    }
}
