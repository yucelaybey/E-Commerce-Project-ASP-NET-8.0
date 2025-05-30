using ECommerce.Dto.AddressDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.AddressQueries
{
    public class GetAddressesByUserIdQuery : IRequest<List<GetAddressDto>>
    {
        public int UserId { get; set; }

        public GetAddressesByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
