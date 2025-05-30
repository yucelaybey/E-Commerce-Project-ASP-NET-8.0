using E_Commerce.Application.Features.Mediator.Queries.DistrictQueries;
using E_Commerce.Application.Interfaces.ILocationRepository;
using ECommerce.Dto.DistrictDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.DistrictHandlers
{
    public class GetDistrictsByQuarterIdQueryHandler : IRequestHandler<GetDistrictsByQuarterIdQuery, List<GetDistrictDto>>
    {
        private readonly ILocationRepository _repository;

        public GetDistrictsByQuarterIdQueryHandler(ILocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetDistrictDto>> Handle(GetDistrictsByQuarterIdQuery request, CancellationToken cancellationToken)
        {
            var districts = await _repository.GetDistrictsByQuarterIdAsync(request.QuarterId);

            return districts.Select(d => new GetDistrictDto
            {
                DistrictId = d.DistrictId,
                CityId = d.CityId,
                TownId = d.TownId,
                QuarterId = d.QuarterId,
                DistrictName = d.DistrictName,
                PostalCode = d.PostalCode
            }).ToList();
        }
    }
}
