using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using ECommerce.Dto.PaymentCardDtos; // GetPaymentCardDto'nun bulunduğu namespace

namespace ECommerce.WebUI.ViewComponents.ProfilesViewComponents
{
    public class _CardSectionProfilesComponentPartial : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _CardSectionProfilesComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
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
                    ViewBag.Error = "Kullanıcı girişi gereklidir";
                    return View(new List<GetPaymentCardDto>());
                }

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Kullanıcı bilgileri alınamadı";
                    return View(new List<GetPaymentCardDto>());
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                string userId = userInfo?.appUserId?.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ViewBag.Error = "Kullanıcı ID'si bulunamadı";
                    return View(new List<GetPaymentCardDto>());
                }

                // 3. Kullanıcı ID'si ile ödeme kartlarını getir
                var paymentCardsUrl = $"https://localhost:7026/api/PaymentCard/user/{userId}";
                var paymentCardsResponse = await _httpClient.GetAsync(paymentCardsUrl);

                if (!paymentCardsResponse.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Ödeme kartları alınamadı";
                    return View(new List<GetPaymentCardDto>());
                }

                var paymentCardsContent = await paymentCardsResponse.Content.ReadAsStringAsync();
                var paymentCards = JsonConvert.DeserializeObject<List<GetPaymentCardDto>>(paymentCardsContent);

                return View(paymentCards ?? new List<GetPaymentCardDto>());
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                return View(new List<GetPaymentCardDto>());
            }
        }
    }
}