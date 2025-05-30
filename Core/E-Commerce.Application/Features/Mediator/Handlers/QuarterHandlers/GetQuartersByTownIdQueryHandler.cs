using E_Commerce.Application.Features.Mediator.Queries.QuarterQueries;
using E_Commerce.Application.Interfaces.ILocationRepository;
using ECommerce.Dto.QuarterDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.QuarterHandlers
{
    public class GetQuartersByTownIdQueryHandler : IRequestHandler<GetQuartersByTownIdQuery, List<GetQuarterDto>>
    {
        private readonly ILocationRepository _repository;

        public GetQuartersByTownIdQueryHandler(ILocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetQuarterDto>> Handle(GetQuartersByTownIdQuery request, CancellationToken cancellationToken)
        {
            var quarters = await _repository.GetQuartersByTownIdAsync(request.TownId);

            return quarters.Select(q => new GetQuarterDto
            {
                QuarterId = q.QuarterId,
                CityId = q.CityId,
                TownId = q.TownId,
                QuarterName = q.QuarterName
            }).ToList();
        }
    }
}
