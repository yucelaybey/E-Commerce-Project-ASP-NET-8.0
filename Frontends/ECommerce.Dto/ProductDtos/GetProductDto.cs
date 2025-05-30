using E_Commerce.Domain.Entities;
using System.Text.Json.Serialization;

namespace ECommerce.Dto.ProductDtos
{
    public class GetProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? SalePrice { get; set; }
        public int Stock { get; set; }
        public int TotalOrder { get; set; }
        public double? DiscountRate { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public string? UserId { get; set; }
        public bool SaleSeason { get; set; }
        public string CategoryName { get; set; }
        public int? FavoritesCardID { get; set; }
        public int? FavoritesCardItemID { get; set; }
        public bool Status { get; set; }
    }
}
