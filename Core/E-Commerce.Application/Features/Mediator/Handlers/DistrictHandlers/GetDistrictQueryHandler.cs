using E_Commerce.Application.Features.Mediator.Queries.DistrictQueries;
using E_Commerce.Application.Features.Mediator.Results.DistrictResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.DistrictHandlers
{
    public class GetDistrictQueryHandler : IRequestHandler<GetDistrictQuery, List<GetDistrictQueryResult>>
    {
        private readonly IRepository<District> _repository;

        public GetDistrictQueryHandler(IRepository<District> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetDistrictQueryResult>> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetDistrictQueryResult
            {
                DistrictId = x.DistrictId,
                DistrictName = x.DistrictName,
                CityId = x.CityId,
                QuarterId = x.QuarterId,
                TownId = x.TownId,
                PostalCode   = x.PostalCode
            }).ToList();
        }
    }
}
