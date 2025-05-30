using ECommerce.Dto.AddressDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.ProfilesViewComponents
{
    public class _AddressesSectionProfilesComponentPartial:ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _AddressesSectionProfilesComponentPartial(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Content("<div class='alert alert-danger'>Kullanıcı girişi gereklidir</div>");
                }

                // 2. Email ile kullanıcı ID'sini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return Content("<div class='alert alert-danger'>Kullanıcı bilgileri alınamadı</div>");
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                string userId = userInfo.appUserId?.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    return Content("<div class='alert alert-danger'>Kullanıcı ID'si bulunamadı</div>");
                }

                // 3. Adres bilgilerini çek
                var addressesResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Addressess/user/{userId}");

                if (!addressesResponse.IsSuccessStatusCode)
                {
                    return View();
                }

                var addressesContent = await addressesResponse.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<GetAddressDto>>(addressesContent);

                if (addresses == null || !addresses.Any())
                {
                    return Content("<div class='alert alert-info'>Kayıtlı adresiniz bulunmamaktadır</div>");
                }

                // 4. View'e gönder
                return View(addresses);
            }
            catch (HttpRequestException ex)
            {
                return Content($"<div class='alert alert-danger'>API bağlantı hatası: {ex.Message}</div>");
            }
            catch (JsonException ex)
            {
                return Content($"<div class='alert alert-danger'>Veri işleme hatası: {ex.Message}</div>");
            }
            catch (Exception ex)
            {
                return Content($"<div class='alert alert-danger'>Beklenmeyen hata: {ex.Message}</div>");
            }
        }
    }
}
