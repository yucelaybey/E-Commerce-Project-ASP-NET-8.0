using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Net.Http;
using ECommerce.Dto.AppUserDto;

public class _UserInfoSectionProfilesComponentPartial : ViewComponent
{
    private readonly HttpClient _httpClient;

    public _UserInfoSectionProfilesComponentPartial(HttpClient httpClient)
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

            // 3. UserId ile detaylı kullanıcı bilgilerini al
            var userDetailsUrl = $"https://localhost:7026/api/AppUser/{userId}";
            var userDetailsResponse = await _httpClient.GetAsync(userDetailsUrl);

            if (!userDetailsResponse.IsSuccessStatusCode)
            {
                return Content("<div class='alert alert-danger'>Kullanıcı detayları alınamadı</div>");
            }

            var userDetailsContent = await userDetailsResponse.Content.ReadAsStringAsync();
            var userDetails = JsonConvert.DeserializeObject<UserProfileDto>(userDetailsContent);

            // 4. DTO'yu view'e gönder
            return View(userDetails);
        }
        catch (Exception ex)
        {
            return Content($"<div class='alert alert-danger'>Bir hata oluştu: {ex.Message}</div>");
        }
    }
}