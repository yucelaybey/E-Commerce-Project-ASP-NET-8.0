using ECommerce.Dto.PaymentCardDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ECommerce.WebUI.Controllers
{
    [Route("CreditCard")]
    public class CreditCardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public CreditCardController(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CreatePaymentCard")]
        public async Task<IActionResult> CreatePaymentCard([FromBody] CreatePaymentCardDto model)
        {
            // 1. Kullanıcı Email'ini al
            var claimsPrincipal = User as ClaimsPrincipal;
            var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Kullanıcı girişi gereklidir" });
            }

            // 2. Email ile kullanıcı bilgilerini al
            var encodedEmail = Uri.EscapeDataString(userEmail);
            var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
            var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı" });
            }

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
            string userId = userInfo.appUserId?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Kullanıcı ID'si bulunamadı" });
            }

            try
            {
                // Kart bilgilerini hazırla
                var cardData = new
                {
                    userId = int.Parse(userId),
                    cardNumber = model.CardNumber,
                    cardHolderName = model.CardHolderName,
                    expirationDate = DateTime.Parse(model.ExpirationDate),
                    cvv = model.CVV,
                    isDefault = model.IsDefault
                };

                // API'ye gönder
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("https://localhost:7026/api/PaymentCard", cardData);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { success = false, message = "Kart kaydedilemedi" });
                }

                // Başarılı yanıtı DTO'ya dönüştür
                var responseContent = await response.Content.ReadFromJsonAsync<CreatedPaymentCardResponseDto>();

                if (responseContent == null || !responseContent.Success)
                {
                    return BadRequest(new { success = false, message = "Kart oluşturuldu ancak beklenmeyen bir yanıt alındı" });
                }

                // PaymentCardId'yi al
                int paymentCardId = responseContent.PaymentCardId;

                // Eğer kart varsayılan olarak işaretlenmişse
                if (model.IsDefault)
                {
                    // DefaultCreditCard action'ını çağır ve sonucunu al
                    var defaultCardResult = await DefaultCreditCard(paymentCardId) as JsonResult;
                    var defaultCardResponse = defaultCardResult?.Value as dynamic;

                    if (!(bool)(defaultCardResponse?.success ?? false))
                    {
                        // Kart güncellendi ama varsayılan yapılamadı
                        return Json(new
                        {
                            success = true, // Güncelleme başarılı olduğu için success=true
                            message = "Kart başarıyla güncellendi ancak varsayılan yapılamadı: " + (string)defaultCardResponse?.message,
                            isDefaultSet = false
                        });
                    }

                    // Her şey başarılı
                    return Json(new
                    {
                        success = true,
                        message = "Kart başarıyla güncellendi ve varsayılan olarak ayarlandı",
                        isDefaultSet = true
                    });
                }

                return Ok(new { success = true, message = "Kart başarıyla kaydedildi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteCreditCard")]
        public async Task<IActionResult> DeleteCreditCard(int id)
        {
            try
            {
                // API'ye istek gönder
                using var httpClient = new HttpClient();
                var response = await httpClient.DeleteAsync($"https://localhost:7026/api/PaymentCard?id={id}");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "API hatası: " + await response.Content.ReadAsStringAsync() });
                }

                return Json(new { success = true, message = "Kart başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UpdateCreditCard")]
        public async Task<IActionResult> UpdateCreditCard([FromBody] UpdatePaymentCardDto model)
        {
            try
            {
                // API'ye istek gönder
                using var httpClient = new HttpClient();
                var response = await httpClient.PutAsJsonAsync("https://localhost:7026/api/PaymentCard", model);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "API hatası: " + await response.Content.ReadAsStringAsync() });
                }

                // Eğer kart varsayılan olarak işaretlenmişse
                if (model.IsDefault)
                {
                    // DefaultCreditCard action'ını çağır ve sonucunu al
                    var defaultCardResult = await DefaultCreditCard(int.Parse(model.PaymentCardID)) as JsonResult;
                    var defaultCardResponse = defaultCardResult?.Value as dynamic;

                    if (!(bool)(defaultCardResponse?.success ?? false))
                    {
                        // Kart güncellendi ama varsayılan yapılamadı
                        return Json(new
                        {
                            success = true, // Güncelleme başarılı olduğu için success=true
                            message = "Kart başarıyla güncellendi ancak varsayılan yapılamadı: " + (string)defaultCardResponse?.message,
                            isDefaultSet = false
                        });
                    }

                    // Her şey başarılı
                    return Json(new
                    {
                        success = true,
                        message = "Kart başarıyla güncellendi ve varsayılan olarak ayarlandı",
                        isDefaultSet = true
                    });
                }

                return Json(new { success = true, message = "Kart başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [Route("DefaultCreditCard")]
        public async Task<IActionResult> DefaultCreditCard(int paymentCardId)
        {
            try
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gereklidir" });
                }

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı" });
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                string userId = userInfo.appUserId?.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Kullanıcı ID'si bulunamadı" });
                }

                // 3. API'ye istek yap
                var requestData = new
                {
                    UserId = userId,
                    PaymentCardId = paymentCardId
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7026/api/PaymentCard/set-default", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Varsayılan kart ayarlanamadı" });
                }

                return Json(new { success = true, message = "Varsayılan kart başarıyla ayarlandı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }
    }
}
