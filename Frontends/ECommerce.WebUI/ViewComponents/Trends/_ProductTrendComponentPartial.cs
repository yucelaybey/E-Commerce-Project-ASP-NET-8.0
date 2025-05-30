using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.LoginDtos;
using ECommerce.Dto.OrderItemDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.Trends
{
    public class _ProductTrendComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ProductTrendComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                int? favoritesCardId = null;

                // 1. Kullanıcının email bilgisini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var email = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    // 2. Email ile kullanıcı bilgilerini çek
                    var userResponse = await client.GetAsync($"https://localhost:7026/api/Login/{email}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var userInfo = JsonConvert.DeserializeObject<LoginResponseDto>(await userResponse.Content.ReadAsStringAsync());

                        // 3. Kullanıcının appUserId'si ile favori kart ID'sini al
                        if (userInfo?.AppUserId > 0)
                        {
                            var favoritesCardResponse = await client.GetAsync($"https://localhost:7026/api/FavoritesCard/by-user/{userInfo.AppUserId}");
                            if (favoritesCardResponse.IsSuccessStatusCode)
                            {
                                var responseContent = await favoritesCardResponse.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(responseContent))
                                {
                                    try
                                    {
                                        var favoritesCards = JsonConvert.DeserializeObject<List<FavoritesCardDto>>(responseContent);
                                        favoritesCardId = favoritesCards?.FirstOrDefault()?.favoritesCardId;

                                        // Eğer FavoritesCardId 0 ise null yap
                                        if (favoritesCardId == 0)
                                        {
                                            favoritesCardId = null;
                                        }
                                    }
                                    catch (JsonException ex)
                                    {
                                        Console.WriteLine($"JSON deserialization error: {ex.Message}");
                                        favoritesCardId = null;
                                    }
                                }
                                else
                                {
                                    favoritesCardId = null;
                                }
                            }
                            else
                            {
                                favoritesCardId = null;
                            }
                        }
                    }
                }

                // 4. BestSeller ürünlerini çek (favoritesCardId varsa gönder)
                var bestSellerUrl = favoritesCardId.HasValue
                    ? $"https://localhost:7026/api/OrderItems/GetBestSellerProduct?favoritesCardId={favoritesCardId.Value}"
                    : "https://localhost:7026/api/OrderItems/GetBestSellerProduct";

                var bestSellerResponse = await client.GetAsync(bestSellerUrl);
                var bestSellerProducts = bestSellerResponse.IsSuccessStatusCode
                    ? JsonConvert.DeserializeObject<List<BestSellerProductDto>>(await bestSellerResponse.Content.ReadAsStringAsync())
                    : new List<BestSellerProductDto>();

                // 5. Kullanıcı bilgilerini tekrar çek (rollerle birlikte)
                UserInfoDto finalUserInfo = null;
                if (!string.IsNullOrEmpty(email))
                {
                    var userResponse = await client.GetAsync($"https://localhost:7026/api/Login/{email}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        finalUserInfo = JsonConvert.DeserializeObject<UserInfoDto>(await userResponse.Content.ReadAsStringAsync());
                        finalUserInfo.Roles = claimsPrincipal.Claims
                            .Where(x => x.Type == ClaimTypes.Role)
                            .Select(x => x.Value)
                            .ToList();
                    }
                }

                return View(new Tuple<List<BestSellerProductDto>, UserInfoDto>(
                    bestSellerProducts ?? new List<BestSellerProductDto>(),
                    finalUserInfo
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return View(new Tuple<List<BestSellerProductDto>, UserInfoDto>(
                    new List<BestSellerProductDto>(),
                    null
                ));
            }
        }
    }
}
