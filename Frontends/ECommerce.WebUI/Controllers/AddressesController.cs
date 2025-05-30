using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using ECommerce.Dto.AddressDto;
using ECommerce.Dto.DistrictDto;
using ECommerce.Dto.QuarterDto;
using ECommerce.Dto.TownDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Controllers
{
    [Route("Addresses")]
    public class AddressesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(IHttpClientFactory httpClientFactory, ILogger<AddressesController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetTowns")]
        public async Task<IActionResult> GetTowns(int cityId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7026/api/Locations/towns/{cityId}");

            if (response.IsSuccessStatusCode)
            {
                var towns = await response.Content.ReadFromJsonAsync<List<GetTownDto>>();
                return Ok(towns);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetQuarters")]
        public async Task<IActionResult> GetQuarters(int townId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7026/api/Locations/quarters/{townId}");

            if (response.IsSuccessStatusCode)
            {
                var quarters = await response.Content.ReadFromJsonAsync<List<GetQuarterDto>>();
                return Ok(quarters);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetDistricts")]
        public async Task<IActionResult> GetDistricts(int quarterId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7026/api/Locations/districts/{quarterId}");

            if (response.IsSuccessStatusCode)
            {
                var districts = await response.Content.ReadFromJsonAsync<List<GetDistrictDto>>();
                return Ok(districts);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("CreateAddress")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto dto)
        {
            var client = _httpClientFactory.CreateClient();

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
            var userInfoResponse = await client.GetAsync(userInfoUrl);

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
            dto.UserId = int.Parse(userId);

            try
            {
                var response = await client.PostAsJsonAsync("https://localhost:7026/api/Addressess", dto);

                if (response.IsSuccessStatusCode)
                {
                    // API'den dönen response'u parse et
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // addressId'yi al
                    int addressId = (int)result.data.addressId;

                    // 2. Eğer isDefault true ise, ayrıca SetDefaultAddress endpoint'ini çağır
                    if (dto.IsDefault)
                    {
                        var defaultAddressRequest = new DefaultAddressRequestDto
                        {
                            addressId = addressId, // API'den gelen addressId'yi kullan
                            UserId = dto.UserId.ToString()
                        };

                        var defaultAddressResponse = await client.PostAsJsonAsync("https://localhost:7026/api/Addressess/set-default", defaultAddressRequest);

                        if (!defaultAddressResponse.IsSuccessStatusCode)
                        {
                            var error = await defaultAddressResponse.Content.ReadAsStringAsync();
                            _logger.LogError("Varsayılan adres ayarlama hatası: {Error}", error);
                        }
                    }

                    return Ok(new
                    {
                        success = true,
                        message = "Adres başarıyla eklendi",
                        data = new { addressId = addressId }
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Adres ekleme hatası: {Error}", error);
                    return BadRequest(new { success = false, message = error });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Adres oluşturma sırasında beklenmeyen hata");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Adres oluşturulurken bir hata oluştu"
                });
            }
        }

        [HttpPost]
        [Route("DeleteAddress")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                // 1. Önce kendi API'mıza istek atıyoruz
                using var httpClient = new HttpClient();
                var apiUrl = $"https://localhost:7026/api/Addressess?id={id}";

                var response = await httpClient.DeleteAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API hatası: {errorContent}" });
                }

                // 2. Başarılı ise
                return Json(new { success = true, message = "Adres başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("GetAddressForEdit")]
        public async Task<IActionResult> GetAddressForEdit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7026/api/Addressess/{id}");

            if (response.IsSuccessStatusCode)
            {
                var address = await response.Content.ReadFromJsonAsync<GetAddressDto>();
                return Ok(address);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto dto)
        {
            var client = _httpClientFactory.CreateClient();

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
            var userInfoResponse = await client.GetAsync(userInfoUrl);

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

            // ModelState kontrolü
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Geçersiz model: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                // 1. Önce adresi güncelle
                var response = await client.PutAsJsonAsync("https://localhost:7026/api/Addressess", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API hatası: {Error}", error);
                    return BadRequest(new { success = false, message = error });
                }

                // 2. Eğer isDefault true ise, ayrıca SetDefaultAddress endpoint'ini çağır
                if (dto.isDefault)
                {
                    var defaultAddressRequest = new DefaultAddressRequestDto
                    {
                        addressId = dto.addressID,
                        UserId = userId
                    };

                    var defaultAddressResponse = await client.PostAsJsonAsync("https://localhost:7026/api/Addressess/set-default", defaultAddressRequest);

                    if (!defaultAddressResponse.IsSuccessStatusCode)
                    {
                        var error = await defaultAddressResponse.Content.ReadAsStringAsync();
                        _logger.LogError("Varsayılan adres ayarlama hatası: {Error}", error);
                        // Varsayılan ayarlama hatasında işlemi iptal etmiyoruz, sadece logluyoruz
                    }
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Adres güncelleme hatası");
                return StatusCode(500, new { success = false, message = "Sunucu hatası" });
            }
        }

        [HttpPost]
        [Route("SetDefaultAddress")]
        public async Task<IActionResult> SetDefaultAddress([FromBody] DefaultAddressRequestDto request)
        {
            using var client = new HttpClient();
            try
            {
                if (string.IsNullOrEmpty(request.addressId.ToString()))
                {
                    return Json(new { success = false, message = "Geçersiz adres ID'si" });
                }

                // 1.Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gereklidir" });
                }

                // 2. Email ile kullanıcı bilgilerini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await client.GetAsync(userInfoUrl);

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

                // API'ye istek gönder
                
                var apiUrl = "https://localhost:7026/api/Addressess/set-default";

                var response = await client.PostAsJsonAsync(apiUrl, new
                {
                    UserId = userId,
                    AddressId = request.addressId
                });

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "API isteği başarısız oldu" });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}