using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands
{
    public class UpdateShoppingCardItemCommand : IRequest
    {
        public int ShoppingCardItemID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } // Ürün Adedi
        public int TotalPrice => Product.Price * Quantity; // Toplam Fiyat Hesaplama
        public bool Status { get; set; }
    }
}
