using ECommerce.Dto.TownDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.TownQueries
{
    public class GetTownsByCityIdQuery : IRequest<List<GetTownDto>>
    {
        public int CityId { get; set; }
    }
}
