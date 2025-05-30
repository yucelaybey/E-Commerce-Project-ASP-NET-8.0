using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderCommands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public string OrderNumber { get; set; }
        public string UserId { get; set; } // Siparişi veren kullanıcı
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public int? OrderPaymentCardId { get; set; }
        public int? OrderPaymentOtherId { get; set; }
        public int OrderPaymentAddressId { get; set; }
        public int TotalAmount { get; set; } // Toplam tutar
        public string OrderStatus { get; set; } // Sipariş durumu
    }
}
