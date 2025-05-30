using E_Commerce.Application.Features.Mediator.Queries.TownQueries;
using E_Commerce.Application.Interfaces.ILocationRepository;
using ECommerce.Dto.TownDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.TownHandlers
{
    public class GetTownsByCityIdQueryHandler : IRequestHandler<GetTownsByCityIdQuery, List<GetTownDto>>
    {
        private readonly ILocationRepository _repository;

        public GetTownsByCityIdQueryHandler(ILocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetTownDto>> Handle(GetTownsByCityIdQuery request, CancellationToken cancellationToken)
        {
            var towns = await _repository.GetTownsByCityIdAsync(request.CityId);

            return towns.Select(t => new GetTownDto
            {
                TownId = t.TownId,
                CityId = t.CityId,
                TownName = t.TownName
            }).ToList();
        }
    }
}
