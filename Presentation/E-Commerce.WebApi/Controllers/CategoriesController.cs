using E_Commerce.Application.Features.Mediator.Commands.CategoryCommands;
using E_Commerce.Application.Features.Mediator.Queries.CategoryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(IMediator mediator, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var values = await _mediator.Send(new GetCategoryQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var value = await _mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok("Kategori Başarıyla Eklendi");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            await _mediator.Send(new RemoveCategoryCommand(id));
            return Ok("Kategori Başarıyla Silindi");
        }

        [HttpPut("{id}")] // id parametresini route'a ekle
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command)
        {
            command.CategoryID = id; // Command'e id'yi set et
            await _mediator.Send(command);
            return Ok("Kategori Başarıyla Güncellendi");
        }

        //Image Post

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            // Çözüm kök dizinine çık ve hedef klasörü belirle
            var solutionRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", ".."); // API projesinden iki seviye yukarı çık
            var targetFolder = Path.Combine(solutionRoot, "Frontends", "ECommerce.WebUI", "wwwroot", "CategoryImages");

            // Klasör yoksa oluştur
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            // Resmin dosya adını belirle (unique olması için Guid kullanabilirsiniz)
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(targetFolder, fileName);

            // Resmi kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Resmin yolunu döndür
            var imageUrl = $"/CategoryImages/{fileName}"; // wwwroot'tan itibaren göreli yol
            return Ok(new { ImageUrl = imageUrl });
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteImage(string fileName)
        {
            try
            {
                // Dosya yolunu oluştur
                var solutionRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", ".."); // API projesinden iki seviye yukarı çık
                var targetFolder = Path.Combine(solutionRoot, "Frontends", "ECommerce.WebUI", "wwwroot", "CategoryImages");
                var filePath = Path.Combine(targetFolder, fileName);

                // Debug: Dosya yolunu logla
                Console.WriteLine($"Dosya yolu: {filePath}");

                // Dosya var mı kontrol et
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("Dosya bulunamadı.");
                    return NotFound("Dosya bulunamadı.");
                }

                // Dosyayı sil
                System.IO.File.Delete(filePath);
                Console.WriteLine("Dosya başarıyla silindi.");

                return Ok("Dosya başarıyla silindi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
