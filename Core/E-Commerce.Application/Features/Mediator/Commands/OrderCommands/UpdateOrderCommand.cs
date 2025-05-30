using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderCommands
{
    public class UpdateOrderCommand : IRequest
    {
        public int OrderID { get; set; }
        public string OrderStatus { get; set; } // Sipariş durumu
    }
}
