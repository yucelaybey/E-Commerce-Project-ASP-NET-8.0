namespace E_Commerce.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SalePrice { get; set; }
        public int Stock { get; set; }
        public double DiscountRate { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public bool SaleSeason { get; set; }
        public List<ShoppingCardItem> ShoppingCardItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
