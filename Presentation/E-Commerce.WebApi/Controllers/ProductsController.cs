using E_Commerce.Application.Features.Mediator.Commands.ProductCommands;
using E_Commerce.Application.Features.Mediator.Queries.CategoryProductListQueries;
using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using ECommerce.Dto.CategoryProductList;
using ECommerce.Dto.ProductDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> ProductsList()
        {
            var values = await _mediator.Send(new GetProductQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(int id)
        {
            var value = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts(CreateProductCommand command)
        {
            await _mediator.Send(command);
            return Ok("Ürün Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProducts(int id)
        {
            await _mediator.Send(new RemoveProductCommand(id));
            return Ok("Ürün Başarıyla Silindi");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducts(int id, [FromBody] UpdateProductCommand command)
        {
            command.ProductID = id;
            await _mediator.Send(command);
            return Ok("Ürün Başarıyla Güncellendi");
        }

        //Image Post

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            // Çözüm kök dizinine çık ve hedef klasörü belirle
            var solutionRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", ".."); // API projesinden iki seviye yukarı çık
            var targetFolder = Path.Combine(solutionRoot, "Frontends", "ECommerce.WebUI", "wwwroot", "ProductImages");

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
            var imageUrl = $"/ProductImages/{fileName}"; // wwwroot'tan itibaren göreli yol
            return Ok(new { ImageUrl = imageUrl });
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteImage(string fileName)
        {
            try
            {
                // Dosya yolunu oluştur
                var solutionRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", ".."); // API projesinden iki seviye yukarı çık
                var targetFolder = Path.Combine(solutionRoot, "Frontends", "ECommerce.WebUI", "wwwroot", "ProductImages");
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

        [HttpGet("by-category-with-cart-info/{categoryId}")]
        public async Task<ActionResult<List<CategoryProductListDto>>> GetByCategoryIdWithCartInfo(
    int categoryId,
    [FromQuery] int? favoritesCardId = null)
        {
            var products = await _mediator.Send(new GetProductsByCategoryIdWithCartInfoQuery
            {
                CategoryId = categoryId,
                FavoritesCardId = favoritesCardId
            });
            return Ok(products);
        }

        [HttpGet("WithCategory/{id}")]
        public async Task<ActionResult<GetProductWithCategoryQueryResult>> GetProductWithCategory(int id)
        {
            var query = new GetProductWithCategoryQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("update-stock")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateProductStockCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response);
        }
    }
}
