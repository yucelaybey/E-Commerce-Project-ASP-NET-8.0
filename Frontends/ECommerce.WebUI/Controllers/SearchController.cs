using System.Security.Claims;
using System.Text;
using E_Commerce.Domain.Entities;
using ECommerce.Dto.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using ECommerce.Dto.CategoryDtos;
using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.FavoritesCardItemDtos;
using ECommerce.Dto.SearchDto;
using ECommerce.Dto.AppUserDto;

namespace ECommerce.WebUI.Controllers
{
    [Route("Search")]
    public class SearchController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://localhost:7026/api/Search/";
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(ApiBaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return Json(new List<Product>());

                var response = await _httpClient.GetAsync($"SearchProduct?searchTerm={Uri.EscapeDataString(searchTerm)}");
                response.EnsureSuccessStatusCode();

                var products = await response.Content.ReadFromJsonAsync<List<ProductWithCategoryDto>>();
                return Json(products ?? new List<ProductWithCategoryDto>());
            }
            catch (Exception ex)
            {
                // Log the error if needed
                return Json(new List<ProductWithCategoryDto>());
            }
        }

        [HttpGet("SearchCategory")]
        public async Task<IActionResult> SearchCategory([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return Json(new List<Category>());

                var response = await _httpClient.GetAsync($"SearchCategory?searchTerm={Uri.EscapeDataString(searchTerm)}");
                response.EnsureSuccessStatusCode();

                var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
                return Json(categories ?? new List<Category>());
            }
            catch (Exception ex)
            {
                // Log the error if needed
                return Json(new List<Category>());
            }
        }

        [HttpGet("Anasayfa")]
        public async Task<IActionResult> Index([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return Redirect("/Anasayfa");
            }

            var normalizedTerm = NormalizeSearchTerm(search);
            var products = await GetProductsWithFavoriteInfo(normalizedTerm);
            var categories = await GetCategories(normalizedTerm);

            // Kullanıcı bilgilerini al
            UserInfoDto userInfo = null;
            if (User.Identity.IsAuthenticated)
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                if (!string.IsNullOrEmpty(encodedEmail))
                {
                    // 2. Email ile kullanıcı bilgilerini al
                    var response = await _httpClient.GetAsync($"https://localhost:7026/api/Login/{Uri.EscapeDataString(userEmail)}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiUserInfo = JsonConvert.DeserializeObject<UserInfoDto>(content);

                        userInfo = new UserInfoDto
                        {
                            NameSurname = apiUserInfo.NameSurname,
                            Email = apiUserInfo.Email,
                            Roles = User.Claims
                                .Where(x => x.Type == ClaimTypes.Role)
                                .Select(x => x.Value)
                                .ToList()
                        };
                    }
                }
            }

            return View("SearchPage", new SearchPageDto
            {
                Products = products,
                Categories = categories,
                SearchTerm = search,
                NormalizedSearchTerm = normalizedTerm,
                UserInfo = userInfo
            });
        }

        public async Task<IActionResult> SearchPage([FromQuery] string search)
        {
            var normalizedSearchTerm = NormalizeSearchTerm(search);
            var products = await GetProductsWithFavoriteInfo(normalizedSearchTerm);
            var categories = await GetCategories(normalizedSearchTerm);

            // Kullanıcı bilgilerini al
            UserInfoDto userInfo = null;
            if (User.Identity.IsAuthenticated)
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                if (!string.IsNullOrEmpty(encodedEmail))
                {
                    // 2. Email ile kullanıcı bilgilerini al
                    var response = await _httpClient.GetAsync($"https://localhost:7026/api/Login/{Uri.EscapeDataString(userEmail)}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiUserInfo = JsonConvert.DeserializeObject<UserInfoDto>(content);

                        userInfo = new UserInfoDto
                        {
                            NameSurname = apiUserInfo.NameSurname,
                            Email = apiUserInfo.Email,
                            Roles = User.Claims
                                .Where(x => x.Type == ClaimTypes.Role)
                                .Select(x => x.Value)
                                .ToList()
                        };
                    }
                }
            }

            var viewModel = new SearchPageDto
            {
                Products = products,
                Categories = categories,
                SearchTerm = search,
                NormalizedSearchTerm = normalizedSearchTerm,
                UserInfo = userInfo
            };

            return View("SearchPage", viewModel);
        }

        private async Task<List<GetProductDto>> GetProductsWithFavoriteInfo(string searchTerm)
        {
            try
            {
                string userId = null;

                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                userId = userInfo.appUserId?.ToString();

                // API'ye istek yap (userId null olabilir)
                var apiUrl = $"https://localhost:7026/api/Search/SearchProductAll?searchTerm={Uri.EscapeDataString(searchTerm)}";

                if (!string.IsNullOrEmpty(userId))
                {
                    apiUrl += $"&userId={Uri.EscapeDataString(userId)}";
                }

                var productResponse = await _httpClient.GetAsync(apiUrl);
                productResponse.EnsureSuccessStatusCode();

                var products = await productResponse.Content.ReadFromJsonAsync<List<GetProductDto>>();
                return products ?? new List<GetProductDto>();
            }
            catch (Exception ex)
            {
                // Hata yönetimi (loglama yapılabilir)
                // ex.LogError(); // Örneğin bir loglama metodu kullanılabilir
                return new List<GetProductDto>();
            }
        }

        private async Task<List<GetCategoryDto>> GetCategories(string searchTerm)
        {
            try
            {
                var response = await _httpClient.GetAsync($"SearchCategoryAll?searchTerm={Uri.EscapeDataString(searchTerm)}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<GetCategoryDto>>() ?? new List<GetCategoryDto>();
            }
            catch
            {
                return new List<GetCategoryDto>();
            }
        }

        private async Task<string> GetUserId()
        {
            try
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return null;
                }

                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                return userInfo.appUserId?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private string NormalizeSearchTerm(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return string.Empty;

            // Küçük harfe çevir
            var normalized = searchTerm.ToLowerInvariant();

            // Türkçe karakterleri İngilizce karşılıklarına çevir
            normalized = normalized
                .Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ö", "o")
                .Replace("ç", "c")
                .Replace("İ", "i")
                .Replace("Ğ", "g")
                .Replace("Ü", "u")
                .Replace("Ş", "s")
                .Replace("Ö", "o")
                .Replace("Ç", "c");

            // Özel karakterleri kaldır
            normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");

            return normalized;
        }
    }
}