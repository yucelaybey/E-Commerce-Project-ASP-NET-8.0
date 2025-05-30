namespace ECommerce.Dto.CategoryProductList
{
    public class CategoryProductListDto
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
        public int TotalQuantityInCart { get; set; } // Sepetteki toplam adet
        public bool? Favorites { get; set; }
        public int? FavoritesCardID { get; set; }
        public int? FavoritesCardItemID { get; set; }
    }
}
