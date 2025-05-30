using E_Commerce.Application.Features.Mediator.Commands.OrderCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.IOrderRepository;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                PaymentMethod = request.PaymentMethod,
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
                OrderPaymentCardId = request?.OrderPaymentCardId,
                OrderPaymentOtherId = request?.OrderPaymentOtherId,
                OrderStatus = request.OrderStatus,
                OrderPaymentAddressId =request.OrderPaymentAddressId,
                OrderNumber = request.OrderNumber


            };

            return await _orderRepository.CreateOrderAsync(order);
        }
    }
}
