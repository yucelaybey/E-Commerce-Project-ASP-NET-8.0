using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerce.Dto.ShoppingCardItemDtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using E_Commerce.Domain.Entities;
using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.FavoritesCardItemDtos;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.WebUI.Controllers
{
    [Authorize]
    [Route("Anasayfa/Sepetim")]
    public class ShoppingController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public ShoppingController(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        [Route("")]
        [HttpGet("ShoppingCard")]
        public async Task<IActionResult> ShoppingCard()
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
                // API'den sepet verilerini çek
                var apiUrl = $"https://localhost:7026/api/ShoppingCardItems/user/{userId}";
                var response = await _httpClient.GetAsync(apiUrl);

                ShoppingCartWithTotalsDto cartItems;

                if (response.IsSuccessStatusCode)
                {
                    cartItems = await response.Content.ReadFromJsonAsync<ShoppingCartWithTotalsDto>();
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Boş sepet oluştur
                    cartItems = new ShoppingCartWithTotalsDto
                    {
                        Items = new List<ShoppingCartItemDto>(),
                        TotalPrice = 0,
                        TotalSalePrice = 0,
                        TotalDiscountAmount = 0,
                        TotalStatus = false,
                        TrueStatusCount = 0
                    };
                    return View(cartItems);
                }
                else
                {
                    TempData["ErrorMessage"] = "Sepet bilgileri alınamadı";
                    return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
                }

                // Favori bilgilerini al
                var favoritesCardUrl = $"https://localhost:7026/api/FavoritesCard/by-user/{userId}";
                var favoritesCardResponse = await _httpClient.GetAsync(favoritesCardUrl);

                if (favoritesCardResponse.IsSuccessStatusCode)
                {
                    var favoritesCardContent = await favoritesCardResponse.Content.ReadAsStringAsync();
                    var favoritesCard = JsonConvert.DeserializeObject<List<FavoritesCardDto>>(favoritesCardContent);

                    // Favori kartı varsa ve boş değilse işlemleri yap
                    if (favoritesCard != null && favoritesCard.Any() && favoritesCard[0].favoritesCardId != 0)
                    {
                        int favoritesCardId = favoritesCard[0].favoritesCardId;

                        // Her bir ürün için favori durumunu kontrol et
                        foreach (var item in cartItems.Items)
                        {
                            var checkResponse = await _httpClient.PostAsJsonAsync(
                                "https://localhost:7026/api/FavoritesCardItem/check-item",
                                new { FavoritesCardID = favoritesCardId, ProductID = item.ProductId });

                            if (checkResponse.IsSuccessStatusCode)
                            {
                                var favoriteResult = await checkResponse.Content.ReadFromJsonAsync<FavoriteCheckResultDto>();
                                item.Favorites = favoriteResult?.Status ?? false;
                                item.FavoritesCardID = favoritesCardId;
                                item.FavoritesCardItemID = favoriteResult?.FavoritesCardItemID;
                            }
                            else
                            {
                                item.Favorites = false;
                                item.FavoritesCardID = favoritesCardId;
                                item.FavoritesCardItemID = null;
                            }
                        }
                    }
                    else
                    {
                        // Favori kartı yoksa tüm ürünler için false olarak işaretle
                        foreach (var item in cartItems.Items)
                        {
                            item.Favorites = false;
                            item.FavoritesCardID = null;
                            item.FavoritesCardItemID = null;
                        }
                    }
                }
                else
                {
                    // Favori kartı alınamazsa tüm ürünler için false olarak işaretle
                    foreach (var item in cartItems.Items)
                    {
                        item.Favorites = false;
                        item.FavoritesCardID = null;
                        item.FavoritesCardItemID = null;
                    }
                }

                return View(cartItems);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItems([FromForm] string ids)
        {
            try
            {
                var idList = JsonConvert.DeserializeObject<List<int>>(ids);

                using (var httpClient = new HttpClient())
                {
                    foreach (var id in idList)
                    {
                        var response = await httpClient.DeleteAsync($"https://localhost:7026/api/ShoppingCardItems?id={id}");
                        if (!response.IsSuccessStatusCode)
                        {
                            TempData["ErrorMessage"] = $"ID: {id} silinemedi!";
                            return RedirectToAction("ShoppingCard");
                        }
                    }

                    TempData["SuccessMessage"] = $"{idList.Count} ürün başarıyla silindi.";
                    return RedirectToAction("ShoppingCard");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return RedirectToAction("ShoppingCard");
            }
        }

        [Route("UpdateQuantity")]
        [HttpPost("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDto model)
        {
            if (model == null)
            {
                return BadRequest("Geçersiz veri");
            }

            try
            {
                // API'yi çağır (ÖRNEK - Kendi API URL'nizi kullanın)
                var apiUrl = $"https://localhost:7026/api/ShoppingCardItems/{model.ShoppingCardItemID}";
                var response = await _httpClient.PutAsJsonAsync(apiUrl, new
                {
                    ShoppingCardItemId = model.ShoppingCardItemID,
                    Quantity = model.Quantity
                });

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }

                return Ok("Ürün Sayısı Güncellendi");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
        [HttpPost("UpdateCartItemStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItemStatus([FromBody] UpdateCartItemStatusDto dto)
        {
            try
            {
                
                var apiResponse = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7026/api/ShoppingCardItems/update-status",
                    new
                    {
                        ShoppingCartItemId = dto.ShoppingCartItemId,
                        Status = dto.Status
                    });

                return apiResponse.IsSuccessStatusCode ? Ok() : BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("Odeme")]
        public IActionResult PaymentPage()
        {
            return View();
        }
    }
}