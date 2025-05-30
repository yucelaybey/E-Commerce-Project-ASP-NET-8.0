using ECommerce.Dto.ShoppingCardItemDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries
{
    public class GetUserCartItemsQuery : IRequest<ShoppingCartWithTotalsDto>
    {
        public string UserId { get; set; }

        public GetUserCartItemsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
