using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using ECommerce.WebUI.Models;
using ECommerce.Dto.OrderDtos;
using ECommerce.Dto.AppUserDto;
using Newtonsoft.Json;

namespace ECommerce.WebUI.ViewComponents.AdminOrder
{
    public class _MainAdminOrderComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _MainAdminOrderComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Sipariş verilerini çek
            var ordersResponse = await client.GetAsync("https://localhost:7026/api/Orders");

            if (!ordersResponse.IsSuccessStatusCode)
            {
                return View(new List<GetOrderDto>());
            }

            var orders = JsonConvert.DeserializeObject<List<GetOrderDto>>(await ordersResponse.Content.ReadAsStringAsync());

            // Her sipariş için kullanıcı bilgilerini al ve NameSurname'i doldur
            foreach (var order in orders)
            {
                var userResponse = await client.GetAsync($"https://localhost:7026/api/AppUser/{order.UserId}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<UserProfileDto>(await userResponse.Content.ReadAsStringAsync());
                    order.NameSurname = user.NameSurname;
                }
                else
                {
                    order.NameSurname = "Bilinmiyor";
                }
            }

            return View(orders);
        }
    }
}