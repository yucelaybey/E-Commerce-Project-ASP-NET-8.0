using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using ECommerce.Dto.ProductShopping;
using System.ComponentModel;

namespace E_Commerce.Web.Controllers
{
    [Route("ProductShoppingCard")]
    public class ProductShoppingCardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductShoppingCardController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;

                // Token'dan email bilgisini al (cookie tabanlı kimlik doğrulaması için)
                var userEmail = claimsPrincipal.Claims
                    .FirstOrDefault(x => x.Type == "Email")?.Value; // Email claim'ini alıyoruz

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gereklidir" });
                }

                var httpClient = _httpClientFactory.CreateClient();

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı" });
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                string userId = userInfo.appUserId?.ToString(); // API'den gelen appUserId'yi al

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Kullanıcı ID'si bulunamadı" });
                }
                var checkCartUrl = $"https://localhost:7026/api/ShoppingCards/check-user-has-cart/{userId}";
                var checkCartResponse = await httpClient.GetAsync(checkCartUrl);

                int? shoppingCardId = 0;
                bool hasShoppingCard = false;

                if (checkCartResponse.IsSuccessStatusCode)
                {
                    var checkCartContent = await checkCartResponse.Content.ReadAsStringAsync();
                    var checkCartResult = JsonConvert.DeserializeObject<CheckCartResult>(checkCartContent);
                    hasShoppingCard = checkCartResult.HasShoppingCard;
                    shoppingCardId = checkCartResult?.ShoppingCardId ?? -1;
                }

                // 2.2. Eğer sepet yoksa oluştur
                if (!hasShoppingCard)
                {
                    var createCartUrl = $"https://localhost:7026/api/ShoppingCards";
                    var createCartResponse = await httpClient.PostAsJsonAsync(createCartUrl, new { UserId = userId });

                    if (!createCartResponse.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Sepet oluşturulamadı" });
                    }

                    // Tekrar sepet ID'sini al
                    checkCartResponse = await httpClient.GetAsync(checkCartUrl);
                    if (checkCartResponse.IsSuccessStatusCode)
                    {
                        var checkCartContent = await checkCartResponse.Content.ReadAsStringAsync();
                        var checkCartResult = JsonConvert.DeserializeObject<CheckCartResult>(checkCartContent);
                        shoppingCardId = checkCartResult.ShoppingCardId;
                    }
                }

                // 3. Ürün sepette var mı kontrol et
                var checkItemUrl = $"https://localhost:7026/api/ShoppingCardItems/exists?productId={request.ProductId}&shoppingCardId={shoppingCardId}";
                var checkItemResponse = await httpClient.GetAsync(checkItemUrl);

                if (checkItemResponse.IsSuccessStatusCode)
                {
                    var itemExists = await checkItemResponse.Content.ReadAsStringAsync() == "true";

                    if (itemExists)
                    {
                        // 3.1. Ürün sepette varsa quantity güncelle
                        var updateQuantityUrl = $"https://localhost:7026/api/ShoppingCardItems/update-quantity";
                        var updateResponse = await httpClient.PutAsJsonAsync(updateQuantityUrl, new
                        {
                            ProductId = request.ProductId,
                            ShoppingCardId = shoppingCardId,
                            QuantityToAdd = request.Quantity
                        });

                        if (updateResponse.IsSuccessStatusCode)
                        {
                            return Json(new { success = true, message = "Ürün adeti güncellendi" });
                        }
                    }
                    else
                    {
                        // 3.2. Ürün sepette yoksa ürün bilgilerini al ve yeni kayıt oluştur
                        var productUrl = $"https://localhost:7026/api/Products/{request.ProductId}";
                        var productResponse = await httpClient.GetAsync(productUrl);

                        if (productResponse.IsSuccessStatusCode)
                        {
                            var productContent = await productResponse.Content.ReadAsStringAsync();
                            var product = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(productContent);

                            // Yeni sepet öğesi oluştur
                            var addItemUrl = $"https://localhost:7026/api/ShoppingCardItems";
                            var addItemResponse = await httpClient.PostAsJsonAsync(addItemUrl, new
                            {
                                ProductID = request.ProductId,
                                ShoppingCardID = shoppingCardId,
                                Quantity = request.Quantity,
                                TotalPrice = product.SaleSeason ? request.Quantity * product.SalePrice : request.Quantity * product.Price,
                                Status = request.Status
                            });

                            if (addItemResponse.IsSuccessStatusCode)
                            {
                                return Json(new { success = true, message = "Ürün sepete eklendi" });
                            }
                        }
                    }
                }

                return Json(new { success = false, message = "Sepet işlemi sırasında bir hata oluştu" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }
    }
}