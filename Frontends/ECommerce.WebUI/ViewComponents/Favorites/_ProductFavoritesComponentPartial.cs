using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ECommerce.WebUI.Models;
using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.FavoritesCardItemDtos;
using NuGet.Protocol.Plugins;
using ECommerce.Dto.LoginDtos;

namespace ECommerce.WebUI.ViewComponents.Favorites
{
    public class _ProductFavoritesComponentPartial : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public _ProductFavoritesComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7026");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // 1. Kullanıcının email bilgisini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var email = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return View(new List<FavoritesCardItemWithProductDto>());
                }

                // 2. Email ile appUserId'yi al
                var appUserId = await GetAppUserIdByEmail(email);
                if (appUserId == 0)
                {
                    return View(new List<FavoritesCardItemWithProductDto>());
                }

                // 3. appUserId ile favoritesCardId'yi al
                var favoritesCardId = await GetFavoritesCardIdByUserId(appUserId);
                if (favoritesCardId == 0)
                {
                    return View(new List<FavoritesCardItemWithProductDto>());
                }

                // 4. favoritesCardId ile favori ürün listesini al
                var favoriteItems = await GetFavoriteItemsByCardId(favoritesCardId);

                return View(favoriteItems);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                return View(new List<FavoritesCardItemWithProductDto>());
            }
        }

        private async Task<int> GetAppUserIdByEmail(string email)
        {
            var response = await _httpClient.GetAsync($"/api/Login/{Uri.EscapeDataString(email)}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResponseDto>(content);
                return result.AppUserId;
            }
            return 0;
        }

        private async Task<int> GetFavoritesCardIdByUserId(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/FavoritesCard/by-user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var favoritesCards = JsonConvert.DeserializeObject<List<FavoritesCardDto>>(content);
                return favoritesCards.FirstOrDefault()?.favoritesCardId ?? 0;
            }
            return 0;
        }

        private async Task<List<FavoritesCardItemWithProductDto>> GetFavoriteItemsByCardId(int cardId)
        {
            var response = await _httpClient.GetAsync($"/api/FavoritesCardItem/by-favorites-card/{cardId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FavoritesCardItemWithProductDto>>(content);
            }
            return new List<FavoritesCardItemWithProductDto>();
        }
    }
}