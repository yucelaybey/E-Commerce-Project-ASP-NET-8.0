using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.LoginDtos;
using ECommerce.Dto.RegisterDto;
using ECommerce.WebUI.SendEmailService;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.WebUI.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EmailService _emailService;

        public LoginController(IHttpClientFactory httpClientFactory, EmailService emailService)
        {
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
        }
        [HttpGet]
        [Route("GirisYap")]
        public IActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        [Route("GirisYap")]
        public async Task<IActionResult> GirisYap(CreateLoginDto createLoginDto)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(createLoginDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7026/api/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonDate = await response.Content.ReadAsStringAsync();
                var tokenModel = System.Text.Json.JsonSerializer.Deserialize<JwtResponseModel>(jsonDate, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (tokenModel != null)
                {
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(tokenModel.Token);
                    var claims = token.Claims.ToList();

                    if (tokenModel.Token != null)
                    {
                        claims.Add(new Claim("accessToken", tokenModel.Token));
                        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                        // Beni Hatırla seçeneğine göre oturum süresini ayarla
                        var authProps = new AuthenticationProperties
                        {
                            IsPersistent = createLoginDto.RememberMe, // Beni Hatırla seçeneği
                            ExpiresUtc = createLoginDto.RememberMe ? DateTime.UtcNow.AddDays(30) : tokenModel.ExpireDate // 30 gün veya token süresi
                        };

                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);
                        return RedirectToAction("Anasayfa", "Main");
                    }
                }
            }

            // Hata durumunda
            ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre");
            return View(createLoginDto);
        }

        [HttpGet]
        [Route("KayitOl")]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        [Route("KayitOl")]
        public async Task<IActionResult> KayitOl(CreateRegisterDto createRegisterDto)
        {
            // Önce e-posta kontrolü yap
            var client = _httpClientFactory.CreateClient();
            var checkEmailResponse = await client.GetAsync($"https://localhost:7026/api/Login/check-email?email={Uri.EscapeDataString(createRegisterDto.Email)}");

            if (checkEmailResponse.IsSuccessStatusCode)
            {
                var content = await checkEmailResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(content);
                    bool emailExists = jsonObject["emailExists"].Value<bool>();

                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı");
                        return View(createRegisterDto);
                    }
                }
            }

            createRegisterDto.EmailConfirm = true;

            // E-posta yoksa kayıt işlemini yap
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(createRegisterDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7026/api/Registers", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("GirisYap");
            }

            // Kayıt işlemi başarısız olursa
            ModelState.AddModelError("", "Kayıt işlemi sırasında bir hata oluştu");
            return View(createRegisterDto);
        }

        [HttpGet]
        [Route("CikisYap")]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcının oturumunu sonlandır
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

            // İsteğe bağlı: Cookie'leri temizle
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Çıkış yapıldıktan sonra yönlendirme yap
            return RedirectToAction("Anasayfa", "Main");
        }

        [HttpGet]
        [Route("SifremiUnuttum")]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        [Route("SifremiUnuttum")]
        public async Task<IActionResult> SifremiUnuttum(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email adresi boş olamaz.");
                return View();
            }

            // API'yi çağır
            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = $"https://localhost:7026/api/Login/check-email?email={email}";

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // API'den dönen yanıtı CheckEmailResponseDto'ya dönüştür
                var result = await response.Content.ReadFromJsonAsync<CheckEmailResponseDto>();

                if (result.EmailExists)
                {
                    // 6 haneli rastgele kod oluştur
                    int code = RandomGenerateNumber.GenerateSixDigitCode();

                    // Email gönder
                    _emailService.SendPasswordResetEmail(result.Email, result.NameSurname, code);

                    // Mail, kod ve kullanıcı bilgilerini MailCodeValid sayfasına gönder
                    var model = new PasswordResetModel
                    {
                        Id = result.Id,
                        Email = result.Email,
                        NameSurname = result.NameSurname,
                        Code = code.ToString()
                    };

                    // Modeli ve kodu TempData'ya kaydet
                    TempData["PasswordResetModel"] = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    TempData["TempCode"] = code.ToString(); // Kodu TempData'ya kaydet
                    TempData["TempMail"] = result.Email.ToString(); // Kodu TempData'ya kaydet
                    TempData["TempName"] = result.NameSurname.ToString(); // Kodu TempData'ya kaydet
                    TempData["TempId"] = result.Id; // Kodu TempData'ya kaydet

                    return RedirectToAction("MailCodeValid");
                }
                else
                {
                    // Email adresi yoksa hata mesajı göster
                    ModelState.AddModelError("", "Böyle bir mail adresi bulunamadı.");
                    return View();
                }
            }
            else
            {
                // API çağrısı başarısız olursa hata mesajı göster
                ModelState.AddModelError("", "API çağrısı sırasında bir hata oluştu.");
                return View();
            }
        }

        [HttpGet]
        [Route("MailCodeValid")]
        public IActionResult MailCodeValid()
        {
            if (TempData["TempCode"] == null)
            {
                return RedirectToAction("SifremiUnuttum");
            }

            TempData.Keep("TempCode");
            return View();
        }

        [HttpPost]
        [Route("MailCodeValid")]
        public IActionResult MailCodeValid(string actionA, string EnteredCode)
        {
            var tempCode = TempData["TempCode"] as string;
            var tempMail = TempData["TempMail"] as string;
            var tempName = TempData["TempName"] as string;

            if (actionA == "VerifyCode")
            {
                if (EnteredCode == tempCode)
                {
                    return RedirectToAction("NewPassword");
                }
                TempData["Error"] = "Girdiğiniz kod hatalı!";
            }
            else if (actionA == "ResendCode")
            {
                int newCode = RandomGenerateNumber.GenerateSixDigitCode();
                _emailService.SendPasswordResetEmail(tempMail, tempName, newCode);
                TempData["TempCode"] = newCode.ToString();
            }

            return RedirectToAction("MailCodeValid");
        }

        [HttpGet]
        [Route("NewPassword")]
        public IActionResult NewPassword()
        {
            // TempData kontrolü daha detaylı
            if (!TempData.ContainsKey("TempId"))
            {
                return RedirectToAction("Error", "Home", new { message = "Geçersiz işlem" });
            }

            var userId = TempData["TempId"] as int?;
            TempData.Keep("TempId"); // Sonraki POST için koru

            return View(new UpdateAppUserPasswordDto());
        }

        [HttpPost]
        [Route("NewPassword")]
        public async Task<IActionResult> NewPassword(UpdateAppUserPasswordDto model)
        {
            try
            {
                // TempData'dan ID'yi al
                if (!TempData.TryGetValue("TempId", out var tempIdObj) || !(tempIdObj is int userId))
                {
                    return RedirectToAction("Error", "Home", new { message = "Kullanıcı bulunamadı" });
                }

                // API isteği
                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync(
                    $"https://localhost:7026/api/AppUser/{userId}",
                    new
                    {
                        Password = model.Password,
                    });

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API hatası: {errorContent}");
                }

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("change-password")]
        [HttpPost("api/auth/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            try
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı girişi gereklidir"
                    });
                }

                // 2. Email ile kullanıcı ID'sini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı bilgileri alınamadı"
                    });
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                string userId = userInfo.appUserId?.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı ID'si bulunamadı"
                    });
                }

                // 3. UserId ile detaylı kullanıcı bilgilerini al
                var userDetailsUrl = $"https://localhost:7026/api/AppUser/{userId}";
                var userDetailsResponse = await _httpClient.GetAsync(userDetailsUrl);

                if (!userDetailsResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı detayları alınamadı"
                    });
                }

                var userDetailsContent = await userDetailsResponse.Content.ReadAsStringAsync();
                var userDetails = JsonConvert.DeserializeObject<UserProfileDto>(userDetailsContent);

                // 4. Eski şifre kontrolü
                if (userDetails.Password != model.CurrentPassword) // Gerçek uygulamada hash karşılaştırması yapılmalı
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Eski şifreniz yanlış"
                    });
                }

                // 5. Yeni şifreyi güncelle
                var updateResponse = await _httpClient.PutAsJsonAsync(
                    $"https://localhost:7026/api/AppUser/{userId}",
                    new
                    {
                        Password = model.NewPassword,
                    });

                if (!updateResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Şifre güncellenirken bir hata oluştu"
                    });
                }

                return Ok(new
                {
                    status = "success",
                    message = "Şifreniz başarıyla güncellendi"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Bir hata oluştu: " + ex.Message
                });
            }
        }

        [Authorize]
        [HttpPost("api/auth/update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto model)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            try
            {
                // 1. Kullanıcı Email'ini al
                var claimsPrincipal = User as ClaimsPrincipal;
                var userEmail = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı girişi gereklidir"
                    });
                }

                // 2. Email ile kullanıcı ID'sini al
                var encodedEmail = Uri.EscapeDataString(userEmail);
                var userInfoUrl = $"https://localhost:7026/api/Login/{encodedEmail}";
                var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);

                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı bilgileri alınamadı"
                    });
                }

                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoContent);
                string userId = userInfo.appUserId?.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Kullanıcı ID'si bulunamadı"
                    });
                }

                // 3. Kullanıcı bilgilerini güncelle
                var updateResponse = await _httpClient.PutAsJsonAsync(
                    $"https://localhost:7026/api/AppUser/UpdateAppUser/{userId}",
                    new
                    {
                        NameSurname = model.NameSurname,
                        Email = model.Email,
                        EmailCheck = model.EmailCheck,
                        PhoneNumber = model.PhoneNumber,
                        BirtDay = model.BirtDay
                    });

                if (!updateResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Profil güncellenirken bir hata oluştu"
                    });
                }

                // 4. E-posta değiştiyse ve doğrulanmamışsa
                if (model.EmailCheck == false)
                {
                    // Kullanıcının şifresini al
                    var userDetailsResponse = await _httpClient.GetAsync($"https://localhost:7026/api/AppUser/{userId}");
                    if (!userDetailsResponse.IsSuccessStatusCode)
                    {
                        return BadRequest(new
                        {
                            status = "error",
                            message = "Kullanıcı detayları alınamadı"
                        });
                    }

                    var userDetailsContent = await userDetailsResponse.Content.ReadAsStringAsync();
                    var userDetails = JsonConvert.DeserializeObject<UserProfileDto>(userDetailsContent);

                    // Kullanıcının oturumunu sonlandır
                    await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

                    // Cookie'leri temizle
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }

                    // Yeni login işlemi için DTO oluştur
                    var loginDto = new
                    {
                        Email = model.Email,
                        Password = userDetails.Password, // Kullanıcının mevcut şifresi
                        RememberMe = true
                    };

                    // Yeni token al
                    var client = _httpClientFactory.CreateClient();
                    var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://localhost:7026/api/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        var tokenModel = JsonConvert.DeserializeObject<JwtResponseModel>(jsonData);

                        if (tokenModel != null)
                        {
                            var handler = new JwtSecurityTokenHandler();
                            var token = handler.ReadJwtToken(tokenModel.Token);
                            var claims = token.Claims.ToList();

                            claims.Add(new Claim("accessToken", tokenModel.Token));
                            var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                            var authProps = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddDays(30)
                            };

                            await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);

                            return Ok(new
                            {
                                status = "success",
                                message = "Profil bilgileriniz güncellendi.",
                                token = tokenModel.Token
                            });
                        }
                    }

                    return BadRequest(new
                    {
                        status = "error",
                        message = "Otomatik giriş yapılamadı. Lütfen manuel olarak giriş yapın."
                    });
                }

                return Ok(new
                {
                    status = "success",
                    message = "Profil bilgileriniz başarıyla güncellendi"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Bir hata oluştu: " + ex.Message
                });
            }
        }
    }
}