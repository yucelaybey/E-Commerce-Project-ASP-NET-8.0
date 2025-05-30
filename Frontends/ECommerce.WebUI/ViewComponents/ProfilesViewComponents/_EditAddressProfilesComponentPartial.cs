using ECommerce.Dto.CityDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ECommerce.WebUI.ViewComponents.ProfilesViewComponents
{
    public class _EditAddressProfilesComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _EditAddressProfilesComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7026/api/City");

            if (response.IsSuccessStatusCode)
            {
                var cities = await response.Content.ReadFromJsonAsync<List<GetCityDto>>();
                return View(cities);
            }

            return View(new List<GetCityDto>());
        }
    }
}
