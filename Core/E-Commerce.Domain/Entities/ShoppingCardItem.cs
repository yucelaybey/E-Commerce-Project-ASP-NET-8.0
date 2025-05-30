namespace E_Commerce.Domain.Entities
{
    public class ShoppingCardItem
    {
        public int ShoppingCardItemID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int ShoppingCardID { get; set; } // Foreign key
        public ShoppingCard ShoppingCard { get; set; } // Navigation property
        public int Quantity { get; set; } // Ürün Adedi
        public int TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}
