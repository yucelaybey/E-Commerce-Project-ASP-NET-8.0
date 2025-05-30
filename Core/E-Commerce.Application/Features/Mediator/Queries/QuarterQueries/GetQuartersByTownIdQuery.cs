using ECommerce.Dto.QuarterDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.QuarterQueries
{
    public class GetQuartersByTownIdQuery : IRequest<List<GetQuarterDto>>
    {
        public int TownId { get; set; }
    }
}
