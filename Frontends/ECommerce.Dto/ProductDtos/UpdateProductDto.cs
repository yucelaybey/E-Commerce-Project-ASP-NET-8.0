using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Dto.ProductDtos
{
    public class UpdateProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? SalePrice { get; set; }
        public int Stock { get; set; }
        public double? DiscountRate { get; set; }
        public int CategoryID { get; set; }
        public string ImageUrl { get; set; }
        public bool SaleSeason { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IFormFile ImageFile { get; set; } // Resim dosyası için
    }
}
