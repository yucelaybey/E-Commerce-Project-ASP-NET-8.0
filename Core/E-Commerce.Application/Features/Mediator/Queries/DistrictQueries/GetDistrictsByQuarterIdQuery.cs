using ECommerce.Dto.DistrictDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.DistrictQueries
{
    public class GetDistrictsByQuarterIdQuery : IRequest<List<GetDistrictDto>>
    {
        public int QuarterId { get; set; }
    }
}
