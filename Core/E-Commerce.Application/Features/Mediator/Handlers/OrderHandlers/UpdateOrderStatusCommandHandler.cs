using E_Commerce.Application.Features.Mediator.Commands.OrderCommands;
using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using E_Commerce.Application.Interfaces.IOrderRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, OrderStatusUpdateQueryResult>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderStatusUpdateQueryResult> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                if (order == null)
                    return new OrderStatusUpdateQueryResult { Success = false, Message = "Sipariş bulunamadı" };

                order.OrderStatus = request.OrderStatus;
                await _orderRepository.UpdateAsync(order);

                return new OrderStatusUpdateQueryResult { Success = true, Message = "Sipariş durumu güncellendi" };
            }
            catch (Exception ex)
            {
                return new OrderStatusUpdateQueryResult { Success = false, Message = $"Hata oluştu: {ex.Message}" };
            }
        }
    }
}
