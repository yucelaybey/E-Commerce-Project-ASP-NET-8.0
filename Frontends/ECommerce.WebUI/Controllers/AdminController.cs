using E_Commerce.Domain.Entities;
using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryDtos;
using ECommerce.Dto.OrderDtos;
using ECommerce.Dto.OrderItemDtos;
using ECommerce.Dto.OrderPaymentAddressDto;
using ECommerce.Dto.ProductDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ECommerce.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("YoneticiPaneli")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Route("Anasayfa")]
        public IActionResult AdminAnasayfa()
        {
            return View();
        }

        [Route("Kategoriler")]
        public IActionResult Category()
        {
            return View();
        }

        [Route("KategoriEkle")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDto createCategoryDto)
        {
            try
            {
                if (createCategoryDto == null || createCategoryDto.Image == null)
                {
                    return Json(new { success = false, message = "Geçersiz veri veya resim." });
                }

                // Resim yükleme
                string imageUrl;
                using (var httpClient = new HttpClient())
                {
                    var formData = new MultipartFormDataContent();
                    var fileContent = new StreamContent(createCategoryDto.Image.OpenReadStream());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(createCategoryDto.Image.ContentType);
                    formData.Add(fileContent, "file", createCategoryDto.Image.FileName);

                    var response = await httpClient.PostAsync("https://localhost:7026/api/Categories/upload", formData);
                    if (!response.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Resim yüklenirken hata oluştu." });
                    }

                    var responseData = await response.Content.ReadAsStringAsync();
                    imageUrl = JsonConvert.DeserializeObject<dynamic>(responseData).imageUrl;
                }

                // Kategori oluşturma
                var categoryData = new
                {
                    Name = createCategoryDto.Name,
                    Description = createCategoryDto.Description,
                    ImageUrl = imageUrl
                };

                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(categoryData), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://localhost:7026/api/Categories", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Kategori oluşturulurken hata oluştu." });
                    }
                }

                return Json(new { success = true, message = "Kategori başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        [Route("KategoriGetir/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7026/api/Categories/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }

                    var category = await response.Content.ReadFromJsonAsync<GetCategoryDto>();
                    return Json(category);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Route("KategoriGuncelle")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                if (updateCategoryDto == null)
                {
                    return Json(new { success = false, message = "Geçersiz veri." });
                }

                string imageUrl = updateCategoryDto.ImageUrl;

                // Yeni resim yüklendi mi?
                if (updateCategoryDto.Image != null)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var formData = new MultipartFormDataContent();
                        var fileContent = new StreamContent(updateCategoryDto.Image.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(updateCategoryDto.Image.ContentType);
                        formData.Add(fileContent, "file", updateCategoryDto.Image.FileName);

                        var uploadResponse = await httpClient.PostAsync("https://localhost:7026/api/Categories/upload", formData);
                        if (!uploadResponse.IsSuccessStatusCode)
                        {
                            return Json(new { success = false, message = "Resim yüklenirken hata oluştu." });
                        }

                        var uploadResult = await uploadResponse.Content.ReadAsStringAsync();
                        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(uploadResult);
                        imageUrl = jsonResponse.imageUrl;
                    }
                }

                // Kategori güncelleme
                var categoryData = new
                {
                    Id = updateCategoryDto.Id,
                    Name = updateCategoryDto.Name,
                    Description = updateCategoryDto.Description,
                    ImageUrl = imageUrl
                };

                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(categoryData), Encoding.UTF8, "application/json");
                    var updateResponse = await httpClient.PutAsync($"https://localhost:7026/api/Categories/{updateCategoryDto.Id}", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Kategori güncellenirken hata oluştu." });
                    }
                }

                return Json(new { success = true, message = "Kategori başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        [Route("KategoriSil/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                // 1. Kategori bilgisini al (resim adını öğrenmek için)
                using (var httpClient = new HttpClient())
                {
                    var getCategoryResponse = await httpClient.GetAsync($"https://localhost:7026/api/Categories/{id}");

                    if (!getCategoryResponse.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Kategori bilgisi alınamadı." });
                    }

                    var jsonResponse = await getCategoryResponse.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<GetCategoryDto>(jsonResponse);

                    // 2. Kategoriyi veritabanından sil
                    var deleteCategoryResponse = await httpClient.DeleteAsync($"https://localhost:7026/api/Categories/Delete?id={id}");

                    if (!deleteCategoryResponse.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Kategori silinirken bir hata oluştu." });
                    }

                    // 3. Resmi sunucudan sil
                    if (!string.IsNullOrEmpty(category.ImageUrl))
                    {
                        var fileName = Path.GetFileName(category.ImageUrl);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var deleteImageResponse = await httpClient.DeleteAsync($"https://localhost:7026/api/Categories/{fileName}");

                            if (!deleteImageResponse.IsSuccessStatusCode)
                            {
                                return Json(new { success = false, message = "Kategori resmi silinirken bir hata oluştu." });
                            }
                        }
                    }

                    return Json(new { success = true, message = "Kategori başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        [Route("Urunler")]
        public IActionResult Product()
        {
            return View();
        }
        [Route("UrunEkle")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto model)
        {
            try
            {
                var imageUrl = await UploadImage(model.ImageFile);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return Json(new { success = false, message = "Resim yüklenirken hata oluştu" });
                }

                var productData = new
                {
                    model.Name,
                    model.Description,
                    model.Stock,
                    model.Price,
                    model.SalePrice,
                    model.SaleSeason,
                    model.DiscountRate,
                    model.CategoryID,
                    ImageUrl = imageUrl
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7026/api/Products", productData);

                return Json(new
                {
                    success = response.IsSuccessStatusCode,
                    message = response.IsSuccessStatusCode ? null : "Ürün eklenirken hata oluştu"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<string> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            using (var content = new MultipartFormDataContent())
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var fileContent = new ByteArrayContent(stream.ToArray());
                    fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(imageFile.ContentType);
                    content.Add(fileContent, "file", imageFile.FileName);

                    var response = await _httpClient.PostAsync("https://localhost:7026/api/Products/upload", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
                        return jsonResponse.imageUrl; // API'den gelen resim URL'sini döndür
                    }
                }
            }

            return null;
        }

        [Route("UrunSil/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var getProductResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Products/{id}");

                if (!getProductResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Ürün bilgisi alınamadı." });
                }

                var jsonResponse = await getProductResponse.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<GetProductDto>(jsonResponse);

                var deleteProductResponse = await _httpClient.DeleteAsync($"https://localhost:7026/api/Products?id={id}");

                if (!deleteProductResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Ürün silinirken bir hata oluştu." });
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var fileName = Path.GetFileName(product.ImageUrl);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var deleteImageResponse = await _httpClient.DeleteAsync($"https://localhost:7026/api/Products/{fileName}");

                        if (!deleteImageResponse.IsSuccessStatusCode)
                        {
                            // Ürün silindi ama resim silinemedi - yine de başarılı sayalım ama uyarı verelim
                            return Json(new
                            {
                                success = true,
                                message = "Ürün silindi ancak resim silinemedi.",
                                reload = true
                            });
                        }
                    }
                }

                // Başarılı yanıt - frontend sayfayı yenileyecek
                return Json(new
                {
                    success = true,
                    message = "Ürün başarıyla silindi.",
                    reload = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Bir hata oluştu: {ex.Message}"
                });
            }
        }

        [Route("UrunGuncelle")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto model)
        {
            try
            {
                string imageUrl = model.ImageUrl;

                if (model.ImageFile != null)
                {
                    imageUrl = await UploadImage(model.ImageFile);
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        return Json(new { success = false, message = "Resim yüklenirken hata oluştu" });
                    }
                }

                var productData = new
                {
                    model.ProductID,
                    model.Name,
                    model.Description,
                    model.Price,
                    model.SalePrice,
                    model.SaleSeason,
                    model.Stock,
                    ImageUrl = imageUrl,
                    model.DiscountRate,
                    model.CategoryID
                };

                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7026/api/Products/{model.ProductID}", productData);

                return Json(new
                {
                    success = response.IsSuccessStatusCode,
                    message = response.IsSuccessStatusCode ? null : "Ürün güncellenirken hata oluştu"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("KategoriGetir")]
        public async Task<IActionResult> GetCategoriesForView()
        {
            try
            {
                // API'den veriyi çek
                var response = await _httpClient.GetAsync("https://localhost:7026/api/Categories");
                response.EnsureSuccessStatusCode();

                // JSON'dan DTO'ya deserialize et
                var jsonString = await response.Content.ReadAsStringAsync();
                var categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetCategoryDto>>(jsonString);

                // View'a gönder
                return Json(categories); // Direkt JSON olarak döndür
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"API bağlantı hatası: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("UrunGetir/{productId}")]
        public async Task<IActionResult> UrunGetir(int productId)
        {
            try
            {

                // Build the API URL
                string apiUrl = $"https://localhost:7026/api/Products/{productId}";

                // Send GET request to the API
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content
                    string content = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<GetProductDto>(content);

                    // Return the product data as JSON
                    return Ok(product);
                }
                else
                {
                    // Handle non-success status codes
                    return StatusCode((int)response.StatusCode,
                        $"API request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                return StatusCode(500, $"API request failed: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors
                return StatusCode(500, $"Failed to parse API response: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [Route("Siparisler")]
        public IActionResult Siparisler()
        {
            return View();
        }

        [HttpPost]
        [Route("SiparisleriGetir")]
        public async Task<IActionResult> SiparisleriGetir([FromBody] OrderRequestDto request)
        {
            try
            {
                // Artık request.OrderId şeklinde erişiyoruz
                var orderId = request.OrderId;

                if (orderId <= 0)
                {
                    return BadRequest("Geçersiz sipariş ID");
                }

                // Get order details
                var orderResponse = await _httpClient.GetAsync($"https://localhost:7026/api/Orders/{orderId}");
                if (!orderResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Sipariş bilgisi alınamadı");
                }

                var order = await orderResponse.Content.ReadFromJsonAsync<GetOrderDto>();

                // Get order items
                var orderItemsResponse = await _httpClient.GetAsync($"https://localhost:7026/api/OrderItems/by-order/{orderId}");
                if (!orderItemsResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Sipariş ürünleri alınamadı");
                }

                var orderItems = await orderItemsResponse.Content.ReadFromJsonAsync<List<GetOrderItemDto>>();

                // Calculate prices
                decimal totalPrice = orderItems.Sum(item => item.Price * item.Quantity);
                decimal totalSalePrice = orderItems.Sum(item =>
                    item.SaleSeason ? item.SalePrice * item.Quantity : item.Price * item.Quantity);
                decimal discountAmount = totalPrice - totalSalePrice;

                // Update order with calculated values
                order.TotalPrice = totalPrice;
                order.TotalSalePrice = totalSalePrice;
                order.DiscountAmount = discountAmount;

                // Get customer info
                var userResponse = await _httpClient.GetAsync($"https://localhost:7026/api/AppUser/{order.UserId}");
                if (!userResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Kullanıcı bilgisi alınamadı");
                }

                var user = await userResponse.Content.ReadFromJsonAsync<UserProfileDto>();

                // Get address info
                var addressResponse = await _httpClient.GetAsync($"https://localhost:7026/api/OrderPaymentAddressess/{order.OrderPaymentAddressId}");
                if (!addressResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Adres bilgisi alınamadı");
                }

                var address = await addressResponse.Content.ReadFromJsonAsync<GetOrderPaymentAddressDto>();

                // Create customer DTO
                var customer = new OrderCustomerDto
                {
                    NameSurname = user.NameSurname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    GetOrderPaymentAddressDto = address
                };

                // Return all data
                return Ok(new
                {
                    order,
                    orderItems,
                    customer
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("SiparisDurumuGuncelle")]
        public async Task<IActionResult> SiparisDurumuGuncelle([FromBody] OrderStatusUpdateDto model)
        {
            try
            {
                // Update order status in the API
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7026/api/Orders/update-status", new
                {
                    orderId = model.orderId,
                    orderStatus = model.newStatus
                });

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest("Sipariş durumu güncellenemedi");
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
