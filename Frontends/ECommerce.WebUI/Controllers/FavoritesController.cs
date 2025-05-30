using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.FavoritesCardItemDtos;
using ECommerce.Dto.LoginDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ECommerce.WebUI.Controllers
{
    [Route("Anasayfa/Favoriler")]
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(
            IHttpClientFactory httpClientFactory,
            ILogger<FavoritesController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [Route("")]
        public IActionResult Favorites()
        {
            return View();
        }

        [HttpPost("FavoriEkle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoritesRequestDto model)
        {

            try
            {
                // 1. Favori kontrolü
                if (!string.IsNullOrEmpty(model.favoritesCardID) && model.favoritesCardID != "null")
                {
                    if (int.TryParse(model.favoritesCardID, out int favoritesCardID))
                    {
                        var checkResponse = await _httpClient.PostAsJsonAsync(
                            "https://localhost:7026/api/FavoritesCardItem/check-item",
                            new { FavoritesCardID = favoritesCardID, ProductID = model.productID });

                        if (checkResponse.IsSuccessStatusCode)
                        {
                            var isAlreadyFavorite = await checkResponse.Content.ReadFromJsonAsync<FavoriteCheckResultDto>();
                            if (isAlreadyFavorite.Status)
                            {
                                return Ok(new
                                {
                                    success = true,
                                    message = "Ürün zaten favorilerinizde",
                                    isAlreadyFavorite = true,
                                    isAlreadyFavorite.FavoritesCardID,
                                    isAlreadyFavorite.FavoritesCardItemID

                                });
                            }
                        }
                        else
                        {
                            var errorContent = await checkResponse.Content.ReadAsStringAsync();
                            _logger.LogError("Favori kontrol API hatası: {Error}", errorContent);
                        }
                    }
                }

                // 2. Kullanıcı bilgilerini al
                var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("Email");
                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });
                }

                // 3. Kullanıcı detaylarını getir
                var userResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Login/{email}");
                if (!userResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)userResponse.StatusCode,
                        new { success = false, message = "Kullanıcı bilgileri alınamadı." });
                }

                var userData = await userResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (userData?.AppUserId == null)
                {
                    return NotFound(new { success = false, message = "Kullanıcı verileri okunamadı." });
                }

                // 4. Favori kartı işlemleri
                int parsedfavoritesCardID;

                if (string.IsNullOrEmpty(model.favoritesCardID) || model.favoritesCardID == "null")
                {
                    // Mevcut kartı kontrol et
                    var getCardResponse = await _httpClient.GetAsync(
                        $"https://localhost:7026/api/FavoritesCard/by-user/{userData.AppUserId}");

                    if (getCardResponse.IsSuccessStatusCode)
                    {
                        var cardData = await getCardResponse.Content.ReadFromJsonAsync<List<FavoritesCardDto>>();
                        var firstCard = cardData?.FirstOrDefault();
                        if (firstCard != null && firstCard.favoritesCardId > 0)
                        {
                            parsedfavoritesCardID = firstCard.favoritesCardId;
                        }
                        else
                        {
                            // Yeni kart oluştur
                            parsedfavoritesCardID = await CreateNewFavoritesCard(userData.AppUserId);
                            if (parsedfavoritesCardID == 0)
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new { success = false, message = "Favori kartı oluşturulamadı." });
                            }
                        }
                    }
                    else
                    {
                        // Yeni kart oluştur
                        parsedfavoritesCardID = await CreateNewFavoritesCard(userData.AppUserId);
                        if (parsedfavoritesCardID == 0)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError,
                                new { success = false, message = "Favori kartı oluşturulamadı." });
                        }
                    }
                }
                else
                {
                    if (!int.TryParse(model.favoritesCardID, out parsedfavoritesCardID))
                    {
                        return BadRequest(new { success = false, message = "Geçersiz favori kartı ID formatı." });
                    }
                }

                // 5. Ürünü favorilere ekle
                var itemResponse = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7026/api/FavoritesCardItem",
                    new { FavoritesCardID = parsedfavoritesCardID, ProductID = model.productID });

                if (!itemResponse.IsSuccessStatusCode)
                {
                    var errorContent = await itemResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Favori ekleme hatası: {Error}", errorContent);
                    return StatusCode((int)itemResponse.StatusCode, new
                    {
                        success = false,
                        message = "Ürün favorilere eklenirken hata oluştu.",
                        details = errorContent
                    });
                }

                var itemData = await itemResponse.Content.ReadFromJsonAsync<FavoritesCardItemDto>();

                return Ok(new
                {
                    success = true,
                    message = "Ürün favorilere eklendi",
                    favoritesCardItemID = itemData?.FavoritesCardItemID ?? 0,
                    parsedfavoritesCardID,
                    isAlreadyFavorite = false
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Favori ekleme sırasında beklenmeyen hata");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Beklenmeyen bir hata oluştu.",
                    details = ex.Message
                });
            }
        }

        // Eksik metodların implementasyonu (örnek)
        private async Task<int> CreateNewFavoritesCard(string userId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7026/api/FavoritesCard",
                    new { AppUserID = userId });

                if (response.IsSuccessStatusCode)
                {
                    var cardData = await response.Content.ReadFromJsonAsync<FavoritesCardDto>();
                    return cardData?.favoritesCardId ?? 0;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<int> CreateNewFavoritesCard(int appUserId)
        {
            try
            {
                var requestData = new { userId = appUserId };
                var createCardResponse = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7026/api/FavoritesCard", requestData);

                if (!createCardResponse.IsSuccessStatusCode)
                {
                    var errorContent = await createCardResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kart oluşturma hatası: {errorContent}");
                    return 0;
                }

                // Oluşturulan kartı getir
                var getCardResponse = await _httpClient.GetAsync(
                    $"https://localhost:7026/api/FavoritesCard/by-user/{appUserId}");

                var jsonContent = await getCardResponse.Content.ReadAsStringAsync();

                if (!getCardResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Kart getirme hatası: {jsonContent}");
                    return 0;
                }

                return await CheckId(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Genel hata: {ex}");
                return 0;
            }
        }
        private async Task<int> CheckId(string jsonContent)
        {
            // Dizi içindeki ilk öğeden ID'yi al
            try
            {
                // 1. Yöntem: JsonDocument ile
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    var root = doc.RootElement;
                    if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                    {
                        return root[0].GetProperty("favoritesCardId").GetInt32();
                    }
                }

                // 2. Yöntem: DTO ile
                var cards = JsonSerializer.Deserialize<List<FavoritesCardDto>>(jsonContent);
                if (cards?.Count > 0)
                {
                    return cards[0].favoritesCardId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON parse hatası: {ex.Message}\nJSON: {jsonContent}");
                return 0;
            }
            Console.WriteLine($"Beklenen veri bulunamadı: {jsonContent}");
            return 0;
        }

        [HttpPost("FavoriSil")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoritesRequestDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.favoritesCardItemID))
                {
                    _logger.LogWarning("Empty favoritesCardItemID in RemoveFromFavorites");
                    return Json(new { success = false, message = "Favori öğe ID'si bulunamadı." });
                }

                if (!int.TryParse(model.favoritesCardItemID, out int itemId))
                {
                    _logger.LogError("Invalid favoritesCardItemID format: {favoritesCardItemID}", model.favoritesCardItemID);
                    return Json(new { success = false, message = "Geçersiz favori öğe ID formatı." });
                }

                // 1. Önce ilgili kaydın hala var olup olmadığını kontrol ediyoruz
                var checkResponse = await _httpClient.GetAsync($"https://localhost:7026/api/FavoritesCardItem/{itemId}");

                if (!checkResponse.IsSuccessStatusCode)
                {
                    // 404 Not Found durumunda kayıt zaten silinmiş demektir
                    if (checkResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Ürün zaten favorilerden çıkarılmış.",
                            alreadyRemoved = true
                        });
                    }

                    var errorContent = await checkResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Check API error: {Error}", errorContent);
                    return Json(new { success = false, message = "Favori kontrolü sırasında hata oluştu." });
                }

                var itemData = await checkResponse.Content.ReadFromJsonAsync<FavoritesCardItemDto>();

                // 2. Eğer gelen veri boşsa veya ID 0 ise kayıt yok demektir
                if (itemData == null || itemData.FavoritesCardItemID == 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Ürün zaten favorilerden çıkarılmış.",
                        alreadyRemoved = true
                    });
                }

                // 3. Kayıt varsa silme işlemini gerçekleştir
                var deleteResponse = await _httpClient.DeleteAsync($"https://localhost:7026/api/FavoritesCardItem?id={itemId}");

                if (!deleteResponse.IsSuccessStatusCode)
                {
                    var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to delete favorite item. Status: {StatusCode}, Response: {ErrorContent}",
                        deleteResponse.StatusCode, errorContent);

                    return Json(new
                    {
                        success = false,
                        message = "Favori silinirken bir hata oluştu.",
                        details = errorContent
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Ürün favorilerden çıkarıldı."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in RemoveFromFavorites");
                return Json(new
                {
                    success = false,
                    message = "Beklenmeyen bir hata oluştu.",
                    details = ex.Message
                });
            }
        }
    }
}