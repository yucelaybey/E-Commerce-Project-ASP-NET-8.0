using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Mediator.Results.ShoppingCardItemResults
{
    public class GetShoppingCardItemByIdQueryResult
    {
        public int ShoppingCardItemID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } // Ürün Adedi
        public int TotalPrice { get; set; } // Toplam Fiyat Hesaplama
        public bool Status { get; set; }
    }
}
