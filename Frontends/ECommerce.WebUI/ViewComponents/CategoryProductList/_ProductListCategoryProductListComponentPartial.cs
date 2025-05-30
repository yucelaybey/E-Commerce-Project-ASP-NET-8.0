using E_Commerce.Domain.Entities;
using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryProductList;
using ECommerce.Dto.FavoritesCardDtos;
using ECommerce.Dto.LoginDtos;
using ECommerce.Dto.OrderItemDtos;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.CategoryProductList
{
    public class _ProductListCategoryProductListComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ProductListCategoryProductListComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                int? favoritesCardId = null;

                // 1. Kullanıcı bilgilerini ve favori kart ID'sini alma işlemi
                var claimsPrincipal = User as ClaimsPrincipal;
                var email = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    // 1a. Kullanıcı bilgilerini al
                    var userResponse = await client.GetAsync($"https://localhost:7026/api/Login/{email}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(await userResponse.Content.ReadAsStringAsync());

                        if (loginResponse?.AppUserId != null)
                        {
                            // 1b. Favori kart bilgisini al
                            var favoritesCardResponse = await client.GetAsync($"https://localhost:7026/api/FavoritesCard/by-user/{loginResponse.AppUserId}");
                            if (favoritesCardResponse.IsSuccessStatusCode)
                            {
                                var favoritesCard = JsonConvert.DeserializeObject<List<FavoritesCardDto>>(await favoritesCardResponse.Content.ReadAsStringAsync());
                                favoritesCardId = favoritesCard?.FirstOrDefault()?.favoritesCardId;
                            }
                        }
                    }
                }

                // 2. Ürünleri çek (favoritesCardId varsa veya yoksa)
                List<CategoryProductListDto> products = new();
                if (categoryId > 0) // categoryId geçerliyse
                {
                    var productUrl = favoritesCardId.HasValue
                        ? $"https://localhost:7026/api/Products/by-category-with-cart-info/{categoryId}?favoritesCardId={favoritesCardId}"
                        : $"https://localhost:7026/api/Products/by-category-with-cart-info/{categoryId}";

                    var productResponse = await client.GetAsync(productUrl);
                    if (productResponse.IsSuccessStatusCode)
                    {
                        products = JsonConvert.DeserializeObject<List<CategoryProductListDto>>(
                            await productResponse.Content.ReadAsStringAsync()) ?? new List<CategoryProductListDto>();
                    }
                }

                // 3. Kullanıcı bilgilerini hazırla (varsa)
                UserInfoDto userInfo = null;
                if (!string.IsNullOrEmpty(email))
                {
                    userInfo = new UserInfoDto
                    {
                        Email = email,
                        NameSurname = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "NameSurname")?.Value,
                        Roles = claimsPrincipal?.Claims
                            .Where(x => x.Type == ClaimTypes.Role)
                            .Select(x => x.Value)
                            .ToList() ?? new List<string>()
                    };
                }

                return View(new Tuple<List<CategoryProductListDto>, UserInfoDto>(
                    products,
                    userInfo
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return View(new Tuple<List<CategoryProductListDto>, UserInfoDto>(
                    new List<CategoryProductListDto>(),
                    null
                ));
            }
        }
    }
}
