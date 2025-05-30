using ECommerce.Dto.ProductDtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    [Route("Kategoriler")] // Controller seviyesinde route tanımla
    public class ProductDetailsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Route("/Anasayfa/Kategoriler/{categoryName}/{productName}")] // Kategori adı ve ürün adı ile erişim
        public async Task<IActionResult> UrunDetay(string categoryName, string productName, [FromQuery] int productId)
        {
            // API'den ürün bilgilerini çek
            var product = await _httpClient.GetFromJsonAsync<ProductWithCategoryDto>($"https://localhost:7026/api/Products/WithCategory/{productId}");
            if (product == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            // URL'deki kategori adı ve ürün adı ile gerçek verileri karşılaştır
            var formattedCategoryName = FormatForUrl(categoryName);
            var formattedProductName = FormatForUrl(productName);

            if (formattedCategoryName != FormatForUrl(product.CategoryName) || formattedProductName != FormatForUrl(product.Name))
            {
                return NotFound(); // URL'deki bilgiler gerçek verilerle uyuşmuyorsa 404 döndür
            }

            // View'e ürün bilgilerini ve kategori ID'sini gönder
            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = product.ProductID; // productId'yi ViewBag ile taşı
            ViewBag.CategoryId = product.CategoryID;
            ViewBag.CategoryName = product.CategoryName;

            // Ürün detay view'ını doğrudan aç
            return View("UrunDetay");
        }

        public string FormatForUrl(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Türkçe büyük harfleri önce küçült
            text = text.ToLower(System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));

            // Tüm Türkçe karakterleri İngilizce karşılıklarıyla değiştir
            var turkishChars = "ığüşöç";
            var englishChars = "igusoc";

            for (int i = 0; i < turkishChars.Length; i++)
            {
                text = text.Replace(turkishChars[i], englishChars[i]);
            }

            // Özel karakterleri temizle ve boşlukları tireye çevir
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = text.Replace(" ", "-");

            // Ardışık tireleri temizle
            text = System.Text.RegularExpressions.Regex.Replace(text, @"-+", "-");
            return text.Trim('-');
        }
    }
}