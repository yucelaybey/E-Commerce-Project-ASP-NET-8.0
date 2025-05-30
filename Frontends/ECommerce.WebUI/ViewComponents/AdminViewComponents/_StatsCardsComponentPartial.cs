using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using ECommerce.Dto.ProductDtos;
using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.OrderDtos;

namespace ECommerce.WebUI.ViewComponents.AdminViewComponents
{
    public class _StatsCardsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _StatsCardsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var dashboardData = new GetAdminDashboadDto();

            try
            {
                // 1. Sipariş verilerini al ve TotalAmount ile TotalOrder'ı hesapla
                var ordersResponse = await client.GetAsync("https://localhost:7026/api/Orders");
                if (ordersResponse.IsSuccessStatusCode)
                {
                    var ordersJson = await ordersResponse.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<GetOrderDto>>(ordersJson);

                    dashboardData.TotalAmount = orders
                        .Where(o => o.OrderStatus != "İptal Edildi")
                        .Sum(o => o.TotalAmount);

                    dashboardData.TotalOrder = orders.Count;
                }

                // 2. Kullanıcı verilerini al ve TotalUser'ı hesapla
                var usersResponse = await client.GetAsync("https://localhost:7026/api/AppUser");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var usersJson = await usersResponse.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<UserProfileDto>>(usersJson);
                    dashboardData.TotalUser = users.Count;
                }

                // 3. Ürün verilerini al ve TotalProduct'ı hesapla
                var productsResponse = await client.GetAsync("https://localhost:7026/api/Products");
                if (productsResponse.IsSuccessStatusCode)
                {
                    var productsJson = await productsResponse.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<GetProductDto>>(productsJson);
                    dashboardData.TotalProduct = products.Count;
                }
            }
            catch (HttpRequestException ex)
            {
                // Hata durumunda loglama yapılabilir
                // Şimdilik boş dashboardData gönderiyoruz
            }

            return View(dashboardData);
        }
    }
}