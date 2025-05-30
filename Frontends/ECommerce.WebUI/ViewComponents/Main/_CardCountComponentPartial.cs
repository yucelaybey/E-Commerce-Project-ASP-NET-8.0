using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents
{
    public class _CardCountComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public _CardCountComponentPartial(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int totalQuantity = 0;
            // 1. Kullanıcı Email'ini al
            var claimsPrincipal = User as ClaimsPrincipal;

            // Token'dan email bilgisini al (cookie tabanlı kimlik doğrulaması için)
            var userEmail = claimsPrincipal.Claims
                .FirstOrDefault(x => x.Type == "Email")?.Value; // Email claim'ini alıyoruz

            if (string.IsNullOrEmpty(userEmail))
            {
                return Content("<script>alert('Kullanıcı girişi gereklidir');</script>");
            }

            var httpClient = _httpClientFactory.CreateClient();
            // 2. Email ile kullanıcı bilgilerini al
            var encodedEmail = Uri.EscapeDataString(userEmail);
            var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
            var userInfoResponse = await httpClient.GetAsync(userInfoUrl);

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                return Content("<div class='error'>Kullanıcı bilgileri alınamadı</div><script>handleError();</script>");
            }

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
            string userId = userInfo.appUserId?.ToString(); // API'den gelen appUserId'yi al

            if (!string.IsNullOrEmpty(userId))
            {
                var response = await httpClient.GetAsync(
                    $"https://localhost:7026/api/ShoppingCards/GetTotalQuantity?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    totalQuantity = result?.TotalQuantity ?? 0;
                }
            }

            return View(totalQuantity); // Sadece int gönderiyoruz
        }

        private class ApiResponse
        {
            public int TotalQuantity { get; set; }
        }
    }
}