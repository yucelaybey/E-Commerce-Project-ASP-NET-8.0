using ECommerce.Dto.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.WebUI.Controllers
{
    [Route("Anasayfa/Kategoriler")] // Controller seviyesinde route tanımla
    public class CategoryProductListController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryProductListController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Route("{categoryId:int}")] // Kategori ID'si ile erişim
        public async Task<IActionResult> Kategoriler(int categoryId)
        {
            // API'den kategori bilgilerini çek
            var category = await _httpClient.GetFromJsonAsync<GetCategoryDto>($"https://localhost:7026/api/Categories/{categoryId}");
            if (category == null)
            {
                return NotFound(); // Kategori bulunamazsa 404 döndür
            }

            // Kategori adını URL'de kullanmak için yönlendirme yap
            var formattedCategoryName = FormatForUrl(category.Name);
            return RedirectToAction("KategorilerByName", new { categoryName = formattedCategoryName });
        }

        [Route("{categoryName}")] // Kategori adı ile erişim
        public async Task<IActionResult> KategorilerByName(string categoryName)
        {
            // API'den tüm kategorileri çek
            var categories = await _httpClient.GetFromJsonAsync<List<GetCategoryDto>>("https://localhost:7026/api/Categories");

            // Kategori adına göre kategori ID'sini bul
            var category = categories?.FirstOrDefault(c => FormatForUrl(c.Name) == categoryName);
            if (category == null)
            {
                return NotFound(); // Kategori bulunamazsa 404 döndür
            }

            // View'e kategori adını ve ID'sini gönder
            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryId = category.CategoryID;

            // Kategoriler view'ını doğrudan aç
            return View("Kategoriler");
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