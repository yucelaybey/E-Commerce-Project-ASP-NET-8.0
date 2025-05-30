using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryDtos;
using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.FavoritesCardItemDtos;
using ECommerce.Dto.LoginDtos;
using ECommerce.Dto.ProductDtos;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _CardRecommendationComponentPartial:ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _CardRecommendationComponentPartial(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // 1. Kullanıcı bilgilerini al (email üzerinden)
                var claimsPrincipal = User as ClaimsPrincipal;
                var email = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;
                int? favoritesCardId = null;
                UserInfoDto userInfo = null;

                // 2. Eğer kullanıcı giriş yapmışsa önce AppUserId'yi al
                if (!string.IsNullOrEmpty(email))
                {
                    // Önce kullanıcı bilgilerini al (Login API'sinden)
                    var loginResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Login/{email}");
                    if (loginResponse.IsSuccessStatusCode)
                    {
                        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

                        // UserInfoDto'yu oluştur
                        userInfo = new UserInfoDto
                        {
                            Email = email,
                            Roles = claimsPrincipal.Claims
                                .Where(x => x.Type == ClaimTypes.Role)
                                .Select(x => x.Value)
                                .ToList()
                        };

                        // Favori kart ID'sini al (AppUserId ile)
                        var favoritesCardResponse = await _httpClient.GetAsync($"https://localhost:7026/api/FavoritesCard/by-user/{loginResult.AppUserId}");
                        if (favoritesCardResponse.IsSuccessStatusCode)
                        {
                            var favoritesCard = await favoritesCardResponse.Content.ReadFromJsonAsync<List<FavoritesCardDto>>();
                            favoritesCardId = favoritesCard?.FirstOrDefault()?.favoritesCardId;
                        }
                    }
                }

                // 3. Tüm ürünleri getir
                var productResponse = await _httpClient.GetAsync("https://localhost:7026/api/Products");
                if (!productResponse.IsSuccessStatusCode)
                {
                    return View("Default", new ProductRecommendationViewModel
                    {
                        Products = new List<GetProductDto>(),
                        UserInfo = null
                    });
                }

                var products = await productResponse.Content.ReadFromJsonAsync<List<GetProductDto>>();
                var randomProducts = GetRandomItems(products, 3);

                // 4. Her ürün için ek bilgileri doldur
                foreach (var product in randomProducts)
                {
                    // Kategori ismini getir
                    if (product.CategoryID > 0)
                    {
                        var categoryResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Categories/{product.CategoryID}");
                        if (categoryResponse.IsSuccessStatusCode)
                        {
                            var category = await categoryResponse.Content.ReadFromJsonAsync<GetCategoryDto>();
                            product.CategoryName = category?.Name ?? "Kategori Yok";
                        }
                    }

                    // Favori durumunu kontrol et (sadece giriş yapmış kullanıcılar için)
                    if (userInfo != null && favoritesCardId != null)
                    {
                        var checkResponse = await _httpClient.PostAsJsonAsync(
                            "https://localhost:7026/api/FavoritesCardItem/check-item",
                            new { FavoritesCardID = favoritesCardId, ProductID = product.ProductID });
                        if (checkResponse.IsSuccessStatusCode)
                        {
                            var checkResult = await checkResponse.Content.ReadFromJsonAsync<FavoriteCheckResultDto>();
                            product.FavoritesCardItemID = checkResult?.FavoritesCardItemID ?? 0;
                            product.Status = checkResult?.Status ?? false;
                        }
                        product.FavoritesCardID = favoritesCardId;
                    }
                }

                // 5. ViewModel oluştur
                return View(new ProductRecommendationViewModel
                {
                    Products = randomProducts,
                    UserInfo = userInfo
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return View("Default", new ProductRecommendationViewModel
                {
                    Products = new List<GetProductDto>(),
                    UserInfo = null
                });
            }
        }

        private List<T> GetRandomItems<T>(List<T> list, int count)
        {
            if (list == null || list.Count == 0 || count <= 0)
                return new List<T>();

            var random = new Random();
            return list.OrderBy(x => random.Next()).Take(count).ToList();
        }
    }
}
