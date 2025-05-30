using ECommerce.Dto.OrderDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderQueries
{
    public class GetOrdersByUserIdQuery : IRequest<List<GetOrderDto>>
    {
        public string UserId { get; set; }

        public GetOrdersByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
