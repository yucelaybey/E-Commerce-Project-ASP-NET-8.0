using ECommerce.Dto.OrderItemDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries
{
    public class GetOrderItemsByOrderIdQuery : IRequest<IEnumerable<GetOrderItemDto>>
    {
        public int OrderId { get; }

        public GetOrderItemsByOrderIdQuery(int orderId)
        {
            OrderId = orderId;
        }
    }
}
