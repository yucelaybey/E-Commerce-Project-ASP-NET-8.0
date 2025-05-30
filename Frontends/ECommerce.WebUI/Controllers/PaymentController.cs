using E_Commerce.Domain.Entities;
using ECommerce.Dto.AddressDto;
using ECommerce.Dto.ErrorDtos;
using ECommerce.Dto.OrderDtos;
using ECommerce.Dto.OrderItemDtos;
using ECommerce.Dto.OrderPaymentAddressDto;
using ECommerce.Dto.OrderPaymentCardDto;
using ECommerce.Dto.OrderPaymentOthersDto;
using ECommerce.Dto.PaymentCardDtos;
using ECommerce.Dto.ProductDtos;
using ECommerce.Dto.ShoppingCardItemDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerce.WebUI.Controllers
{
    [Authorize]
    [Route("/Anasayfa/Sepetim/")]
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PaymentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("PaymentOrderSave")]
        public async Task<IActionResult> PaymentOrderSave([FromBody] PaymentOrderRequestDto request)
        {
            try
            {
                // 1. Get user email and info
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gereklidir" });
                }

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

                // 2. Process payment based on method
                int? orderPaymentCardId = null;
                int? orderPaymentOtherId = null;

                if (request.PaymentMethod == "Kredi/Banka Kartı")
                {
                    var cardId = request.PaymentData.CardId?.ToString()
                   ?? request.PaymentData.CardId?.ToString();

                    if (cardId != null)
                    {
                        // Saved card
                        var savedCardUrl = $"https://localhost:7026/api/PaymentCard/{cardId}";
                        var savedCardResponse = await _httpClient.GetAsync(savedCardUrl);

                        if (!savedCardResponse.IsSuccessStatusCode)
                        {
                            return Json(new { success = false, message = "Kayıtlı kart bilgileri alınamadı" });
                        }

                        var savedCardContent = await savedCardResponse.Content.ReadAsStringAsync();
                        var savedCard = JsonConvert.DeserializeObject<GetPaymentCardDto>(savedCardContent);

                        var cardPaymentData = new
                        {
                            orderCardNumber = savedCard.CardNumber,
                            orderCardDate = savedCard.ExpirationDate,
                            orderCardCVV = savedCard.CVV,
                            orderCardName = savedCard.CardHolderName
                        };

                        var cardPaymentResponse = await _httpClient.PostAsJsonAsync(
                            "https://localhost:7026/api/OrderPaymentCards", cardPaymentData);

                        if (!cardPaymentResponse.IsSuccessStatusCode)
                        {
                            return Json(new { success = false, message = "Kart bilgileri kaydedilemedi" });
                        }

                        var cardPaymentResult = await cardPaymentResponse.Content.ReadAsStringAsync();
                        orderPaymentCardId = JsonConvert.DeserializeObject<int>(cardPaymentResult);
                    }
                    else
                    {
                        DateTime parsedDate;
                        // MM/yy formatını parse ediyoruz
                        if (DateTime.TryParseExact(request.PaymentData.ExpiryDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            // Her zaman bu yüzyılı kullanmak yerine, iki haneli yılları şu anki yüzyıl + 1 olarak yorumla
                            int currentYear = DateTime.Now.Year;
                            if (parsedDate.Year < currentYear)
                            {
                                parsedDate = parsedDate.AddYears(100);
                            }

                            // Kartın son kullanma tarihi ayın son günü olarak kabul edilir
                            parsedDate = new DateTime(parsedDate.Year, parsedDate.Month,
                                                    DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month));
                        }
                        // New credit card
                        var cardPaymentData = new
                        {
                            orderCardNumber = request.PaymentData.CardNumber,
                            orderCardDate = parsedDate,
                            orderCardCVV = request.PaymentData.CVV,
                            orderCardName = request.PaymentData.CardHolderName
                        };

                        var cardPaymentResponse = await _httpClient.PostAsJsonAsync(
                            "https://localhost:7026/api/OrderPaymentCards", cardPaymentData);

                        if (!cardPaymentResponse.IsSuccessStatusCode)
                        {
                            return Json(new { success = false, message = "Kart bilgileri kaydedilemedi" });
                        }

                        var cardPaymentResult = await cardPaymentResponse.Content.ReadAsStringAsync();
                        orderPaymentCardId = JsonConvert.DeserializeObject<int>(cardPaymentResult);

                        // Save card if user selected the option
                        if (request.PaymentData.SaveCard)
                        {
                            var saveCardData = new
                            {
                                UserId = userId,
                                CardNumber = request.PaymentData.CardNumber,
                                ExpirationDate = parsedDate,
                                CVV = request.PaymentData.CVV,
                                CardHolderName = request.PaymentData.CardHolderName,
                                IsDefault = false
                            };

                            var paymentCardResponse = await _httpClient.PostAsJsonAsync(
                                "https://localhost:7026/api/PaymentCard", saveCardData);

                            if (!paymentCardResponse.IsSuccessStatusCode)
                            {
                                return Json(new { success = false, message = "Kart bilgileri kaydedilemedi" });
                            }
                        }
                    }
                }
                else if (request.PaymentMethod == "Paypal" || request.PaymentMethod == "ApplePay")
                {
                    var otherPaymentData = new
                    {
                        paymentName = request.PaymentMethod,
                        paymentNumber = request.PaymentData.PaymentNumber
                    };

                    var otherPaymentResponse = await _httpClient.PostAsJsonAsync(
                        "https://localhost:7026/api/OrderPaymentOthers", otherPaymentData);

                    if (!otherPaymentResponse.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Ödeme bilgileri kaydedilemedi" });
                    }

                    var otherPaymentResult = await otherPaymentResponse.Content.ReadAsStringAsync();
                    orderPaymentOtherId = JsonConvert.DeserializeObject<int>(otherPaymentResult);
                }

                // 1. Adres bilgisini kontrol et
                if (request.SelectedAddressId <= 0)
                {
                    return Json(new { success = false, message = "Geçerli bir adres seçmelisiniz" });
                }

                // 2. Adres bilgisini API'den al
                var addressResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Addressess/{request.SelectedAddressId}");
                if (!addressResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Geçersiz adres bilgisi" });
                }

                var addressContent = await addressResponse.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<GetAddressDto>(addressContent);

                // 3. Sipariş adresini oluştur
                var orderAddress = new
                {
                    addressTitle = address.AddressTitle,
                    nameSurname = address.NameSurname,
                    phoneNumber = address.PhoneNumber,
                    city = address.City,
                    town = address.Town,
                    quarter = address.Quarter,
                    district = address.District,
                    addressDetails = address.AddressDetails
                };

                // 4. OrderPaymentAddresses API'sine gönder
                var addressApiResponse = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7026/api/OrderPaymentAddressess", orderAddress);

                if (!addressApiResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Adres bilgileri kaydedilemedi" });
                }

                var addressApiResult = await addressApiResponse.Content.ReadAsStringAsync();
                var orderAddressId = JsonConvert.DeserializeObject<int>(addressApiResult);

                // 3. Create order
                var orderNumber = GenerateOrderNumber();
                var orderData = new
                {
                    OrderNumber = orderNumber,
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    PaymentMethod = request.PaymentMethod,
                    OrderPaymentCardId = orderPaymentCardId,
                    OrderPaymentOtherId = orderPaymentOtherId,
                    OrderPaymentAddressId = orderAddressId,
                    TotalAmount = request.CartData.TotalSalePrice,
                    OrderStatus = "Onay Bekleniyor"
                };

                var orderResponse = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7026/api/Orders", orderData);

                if (!orderResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Sipariş oluşturulamadı" });
                }

                var orderResult = await orderResponse.Content.ReadAsStringAsync();
                var orderId = JsonConvert.DeserializeObject<int>(orderResult);

                // 4. Create order items
                foreach (var item in request.CartData.Items)
                {
                    if (!item.Status) continue;

                    // Get product category
                    var productUrl = $"https://localhost:7026/api/Products/{item.ProductId}";
                    var productResponse = await _httpClient.GetAsync(productUrl);

                    if (!productResponse.IsSuccessStatusCode)
                    {
                        continue; // Skip if product not found
                    }

                    var productContent = await productResponse.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<GetProductDto>(productContent);

                    var orderItemData = new
                    {
                        OrderID = orderId,
                        OrderDate = DateTime.Now,
                        ProductID = item.ProductId,
                        Name = item.ProductName,
                        Description = item.ProductDescription,
                        Price = item.ProductPrice,
                        SalePrice = item.ProductSalePrice,
                        Stock = item.ProductStock,
                        DiscountRate = item.ProductDiscountRate,
                        ImageUrl = item.ProductImageUrl,
                        CategoryID = product.CategoryID,
                        SaleSeason = item.ProductSaleSeason,
                        Quantity = item.Quantity,
                        OrderStatus = "ONAY BEKLENİYOR"
                    };

                    await _httpClient.PostAsJsonAsync(
                        "https://localhost:7026/api/OrderItems", orderItemData);

                    var newStock = new
                    {
                        productId = orderItemData.ProductID,
                        newStock = orderItemData.Stock - orderItemData.Quantity
                    };

                    var stockResponse = await _httpClient.PutAsJsonAsync(
                    "https://localhost:7026/api/Products/update-stock", newStock);
                }

                // 5. Clear the shopping cart
                var cartUrl = $"https://localhost:7026/api/ShoppingCardItems/user/{userId}";
                var cartItemsResponse = await _httpClient.GetAsync(cartUrl);

                if (cartItemsResponse.IsSuccessStatusCode)
                {
                    var cartItemsContent = await cartItemsResponse.Content.ReadAsStringAsync();
                    var cartItems = JsonConvert.DeserializeObject<ShoppingCartWithTotalsDto>(cartItemsContent);

                    foreach (var item in cartItems.Items)
                    {
                        await _httpClient.DeleteAsync($"https://localhost:7026/api/ShoppingCardItems/?id={item.ShoppingCartItemID}");
                    }
                }

                return Json(new { success = true, orderId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        private string GenerateOrderNumber()
        {
            var now = DateTime.Now;
            var random = new Random();
            var randomPart = random.Next(10000, 99999).ToString();
            return $"#ORDER-{now:dd}{now:MM}{now:yy}{randomPart}";
        }

        [HttpGet("GetCartData")]
        public async Task<IActionResult> GetCartData()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userEmail = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Kullanıcı girişi gereklidir" });
            }

            var userInfoUrl = $"https://localhost:7026/api/Login/{userEmail}";
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

            var cartUrl = $"https://localhost:7026/api/ShoppingCardItems/user/{userId}";
            var cartResponse = await _httpClient.GetAsync(cartUrl);

            if (!cartResponse.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Sepet bilgileri alınamadı" });
            }

            var cartContent = await cartResponse.Content.ReadAsStringAsync();

            // Direkt olarak API'den gelen ShoppingCartWithTotalsDto formatını kullanıyoruz
            var cartData = JsonConvert.DeserializeObject<ShoppingCartWithTotalsDto>(cartContent);

            return Json(new { success = true, data = cartData });
        }

        [HttpGet("SiparisOnaylandi")]
        public async Task<IActionResult> Confirmation([FromQuery] int orderId)
        {
            try
            {
                // 1. Sipariş bilgilerini al
                var orderResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Orders/{orderId}");
                if (!orderResponse.IsSuccessStatusCode)
                {
                    return View("Error", new ErrorDto { Message = "Sipariş bilgileri alınamadı" });
                }

                var order = await orderResponse.Content.ReadFromJsonAsync<GetOrderDto>();
                if (order == null)
                {
                    return View("Error", new ErrorDto { Message = "Sipariş bilgileri geçersiz" });
                }

                // 2. Adres bilgilerini al
                GetOrderPaymentAddressDto addressInfo = null;
                if (order.OrderPaymentAddressId > 0)
                {
                    var addressResponse = await _httpClient.GetAsync($"https://localhost:7026/api/OrderPaymentAddressess/{order.OrderPaymentAddressId}");
                    if (addressResponse.IsSuccessStatusCode)
                    {
                        addressInfo = await addressResponse.Content.ReadFromJsonAsync<GetOrderPaymentAddressDto>();
                    }
                }

                // 3. Ödeme bilgilerini al
                PaymentInfoDto paymentInfo = null;
                if (order.PaymentMethod == "Paypal" || order.PaymentMethod == "ApplePay")
                {
                    if (order.OrderPaymentOtherId.HasValue)
                    {
                        var paymentOtherResponse = await _httpClient.GetAsync($"https://localhost:7026/api/OrderPaymentOthers/{order.OrderPaymentOtherId}");
                        if (paymentOtherResponse.IsSuccessStatusCode)
                        {
                            var paymentOther = await paymentOtherResponse.Content.ReadFromJsonAsync<GetOrderPaymentOthersDto>();
                            paymentInfo = new PaymentInfoDto
                            {
                                PaymentNumber = paymentOther?.PaymentNumber,
                                PaymentType = order.PaymentMethod
                            };
                        }
                    }
                }
                else if (order.OrderPaymentCardId.HasValue)
                {
                    var paymentCardResponse = await _httpClient.GetAsync($"https://localhost:7026/api/OrderPaymentCards/{order.OrderPaymentCardId}");
                    if (paymentCardResponse.IsSuccessStatusCode)
                    {
                        var paymentCard = await paymentCardResponse.Content.ReadFromJsonAsync<GetOrderPaymentCardDto>();
                        paymentInfo = new PaymentInfoDto
                        {
                            PaymentNumber = paymentCard != null ? MaskCreditCard(paymentCard.OrderCardNumber) : null,
                            PaymentType = "Kredi/Banka Kartı"
                        };
                    }
                }

                // 4. Kullanıcı bilgilerini al
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

                // 5. Sipariş öğelerini al
                List<GetOrderItemDto> orderItems = new List<GetOrderItemDto>();
                var orderItemsResponse = await _httpClient.GetAsync($"https://localhost:7026/api/OrderItems/by-order/{order.OrderID}");

                if (orderItemsResponse.IsSuccessStatusCode)
                {
                    orderItems = await orderItemsResponse.Content.ReadFromJsonAsync<List<GetOrderItemDto>>();
                }

                var totals = CalculateOrderTotals(orderItems);

                var viewModel = new OrderConfirmationDto
                {
                    Order = order,
                    PaymentInfo = paymentInfo,
                    GetOrderItemDto = orderItems,
                    UserEmail = userEmail,
                    AddressInfo = addressInfo,
                    Totals = totals
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorDto { Message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        private string MaskCreditCard(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return cardNumber;

            return new string('*', cardNumber.Length - 4) + cardNumber.Substring(cardNumber.Length - 4);
        }
        private OrderTotalsDto CalculateOrderTotals(List<GetOrderItemDto> orderItems)
        {
            var totals = new OrderTotalsDto
            {
                TotalPrice = orderItems.Sum(oi => oi.Price * oi.Quantity),
                TotalSalePrice = orderItems.Sum(oi => oi.SaleSeason ? oi.SalePrice * oi.Quantity : oi.Price * oi.Quantity),
                TotalDiscountAmount = orderItems.Sum(oi => oi.SaleSeason ? (oi.Price - oi.SalePrice) * oi.Quantity : 0),
                TrueStatusCount = orderItems.Count(oi => oi.OrderStatus == "Completed"),
                Items = orderItems
            };

            totals.TotalDiscountRate = (double)(totals.TotalPrice > 0 ?
    Math.Round((decimal)(totals.TotalDiscountAmount / totals.TotalPrice * 100), 2) : 0);
            totals.TotalStatus = totals.TrueStatusCount > 0 && totals.TrueStatusCount == totals.Items.Count;

            return totals;
        }
    }
}
