using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.FavoritesCardItemDtos;
using ECommerce.Dto.ShoppingCardItemDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _OrderSummaryComponentPartial : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _OrderSummaryComponentPartial(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. Kullanıcı Email'ini al
            var claimsPrincipal = User as ClaimsPrincipal;
            var userEmail = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
            }

            // 2. Email ile kullanıcı bilgilerini al
            var encodedEmail = Uri.EscapeDataString(userEmail);
            var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
            var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
            }

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
            string userId = userInfo.appUserId?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
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
                    return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
                }

                // Favori bilgilerini al
                var favoritesCardUrl = $"https://localhost:7026/api/FavoritesCard/by-user/{userId}";
                var favoritesCardResponse = await _httpClient.GetAsync(favoritesCardUrl);

                if (favoritesCardResponse.IsSuccessStatusCode)
                {
                    var favoritesCardContent = await favoritesCardResponse.Content.ReadAsStringAsync();
                    var favoritesCard = JsonConvert.DeserializeObject<List<FavoritesCardDto>>(favoritesCardContent);
                    int favoritesCardId = favoritesCard.FirstOrDefault()?.favoritesCardId ?? 0;

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

                return View(cartItems);
            }
            catch (Exception)
            {
                return View(new ShoppingCartWithTotalsDto { Items = new List<ShoppingCartItemDto>() });
            }
        }
    }
}
