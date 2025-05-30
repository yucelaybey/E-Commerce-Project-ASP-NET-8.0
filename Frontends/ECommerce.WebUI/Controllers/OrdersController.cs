using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ECommerce.WebUI.Models;
using ECommerce.Dto.OrderDtos;
using ECommerce.Dto.OrderItemDtos;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.WebUI.Controllers
{
    [Authorize]
    [Route("Anasayfa/Siparislerim")]
    public class OrdersController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [Route("")]
        public async Task<IActionResult> Siparislerim()
        {
            // 1. Kullanıcı Email'ini al
            var claimsPrincipal = User as ClaimsPrincipal;
            var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Email ile kullanıcı bilgilerini al
            var encodedEmail = Uri.EscapeDataString(userEmail);
            var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
            var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri alınamadı";
                return View(new List<OrderWithItemsDto>());
            }

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
            string userId = userInfo.appUserId?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Kullanıcı ID'si bulunamadı";
                return View(new List<OrderWithItemsDto>());
            }

            // 3. Kullanıcının siparişlerini al
            var ordersUrl = $"https://localhost:7026/api/Orders/user/{userId}";
            var ordersResponse = await _httpClient.GetAsync(ordersUrl);

            if (!ordersResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Sipariş bilgileri alınamadı";
                return View(new List<OrderWithItemsDto>());
            }

            var ordersContent = await ordersResponse.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<GetOrderDto>>(ordersContent);

            // 4. Her sipariş için sipariş öğelerini al ve birleştir
            var ordersWithItems = new List<OrderWithItemsDto>();

            foreach (var order in orders)
            {
                var orderItemsUrl = $"https://localhost:7026/api/OrderItems/by-order/{order.OrderID}";
                var orderItemsResponse = await _httpClient.GetAsync(orderItemsUrl);

                List<GetOrderItemDto> orderItems = new List<GetOrderItemDto>();

                if (orderItemsResponse.IsSuccessStatusCode)
                {
                    var orderItemsContent = await orderItemsResponse.Content.ReadAsStringAsync();
                    orderItems = JsonConvert.DeserializeObject<List<GetOrderItemDto>>(orderItemsContent);
                }

                // Hesaplamaları yap
                decimal totalPrice = 0;
                decimal totalSalePrice = 0;
                decimal discountAmount = 0;

                foreach (var item in orderItems)
                {
                    totalPrice += item.Price * item.Quantity;
                    totalSalePrice += item.SalePrice * item.Quantity;

                    if (item.SaleSeason)
                    {
                        discountAmount += (item.Price - item.SalePrice) * item.Quantity;
                    }
                }

                // Order nesnesini güncelle
                order.TotalPrice = totalPrice;
                order.TotalSalePrice = totalSalePrice;
                order.DiscountAmount = discountAmount;

                ordersWithItems.Add(new OrderWithItemsDto
                {
                    Order = order,
                    OrderItems = orderItems
                });
            }

            return View(ordersWithItems);
        }
    }
}