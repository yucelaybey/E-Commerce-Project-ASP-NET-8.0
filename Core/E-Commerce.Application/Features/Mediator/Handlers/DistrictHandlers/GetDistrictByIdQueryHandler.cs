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
    public class GetDistrictByIdQueryHandler : IRequestHandler<GetDistrictByIdQuery, GetDistrictByIdQueryResult>
    {
        private readonly IRepository<District> _repository;

        public GetDistrictByIdQueryHandler(IRepository<District> repository)
        {
            _repository = repository;
        }

        public async Task<GetDistrictByIdQueryResult> Handle(GetDistrictByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetDistrictByIdQueryResult
            {
               DistrictId = values.DistrictId,
               DistrictName = values.DistrictName,
               CityId = values.CityId,
                QuarterId = values.QuarterId,
               PostalCode = values.PostalCode,
               TownId    = values.TownId
            };
        }
    }
}
