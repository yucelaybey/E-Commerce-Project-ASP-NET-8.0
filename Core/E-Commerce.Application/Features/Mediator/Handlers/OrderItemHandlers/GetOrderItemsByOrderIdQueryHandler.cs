using E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries;
using E_Commerce.Application.Interfaces.IOrderItemRepository;
using ECommerce.Dto.OrderItemDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class GetOrderItemsByOrderIdQueryHandler : IRequestHandler<GetOrderItemsByOrderIdQuery, IEnumerable<GetOrderItemDto>>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public GetOrderItemsByOrderIdQueryHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<GetOrderItemDto>> Handle(
            GetOrderItemsByOrderIdQuery request,
            CancellationToken cancellationToken)
        {
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(request.OrderId);

            return orderItems.Select(oi => new GetOrderItemDto
            {
                OrderItemID = oi.OrderItemID,
                OrderID = oi.OrderID,
                OrderDate = oi.OrderDate,
                ProductID = oi.ProductID,
                Name = oi.Name,
                Description = oi.Description,
                Price = oi.Price,
                SalePrice = oi.SalePrice,
                Stock = oi.Stock,
                DiscountRate = oi.DiscountRate,
                ImageUrl = oi.ImageUrl,
                CategoryID = oi.CategoryID,
                SaleSeason = oi.SaleSeason,
                Quantity = oi.Quantity,
                OrderStatus = oi.OrderStatus
            });
        }
    }
}
