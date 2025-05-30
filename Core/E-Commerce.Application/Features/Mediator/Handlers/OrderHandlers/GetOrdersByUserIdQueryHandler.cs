using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using E_Commerce.Application.Interfaces.IOrderRepository;
using ECommerce.Dto.OrderDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<GetOrderDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByUserIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<GetOrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId);

            return orders.Select(o => new GetOrderDto
            {
                OrderID = o.OrderID,
                OrderNumber = o.OrderNumber,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod,
                OrderStatus = o.OrderStatus,
                OrderPaymentCardId = o.OrderPaymentCardId,
                OrderPaymentOtherId = o.OrderPaymentOtherId,
                OrderPaymentAddressId = o.OrderPaymentAddressId,
                TotalAmount = o.TotalAmount
            }).ToList();
        }
    }
}
