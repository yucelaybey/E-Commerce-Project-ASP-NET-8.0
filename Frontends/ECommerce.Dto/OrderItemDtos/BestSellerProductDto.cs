using E_Commerce.Domain.Entities;

namespace ECommerce.Dto.OrderItemDtos
{
    public class BestSellerProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? SalePrice { get; set; }
        public int Stock { get; set; }
        public double? DiscountRate { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool SaleSeason { get; set; }
        public int TotalQuantity { get; set; }
        public bool? Favorites { get; set; }
        public int? FavoritesCardID { get; set; }
        public int? FavoritesCardItemID { get; set; }
    }
}
