using E_Commerce.Application.Features.Mediator.Queries.TownQueries;
using E_Commerce.Application.Features.Mediator.Results.TownResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.TownHandlers
{
    public class GetDistrictQueryHandler : IRequestHandler<GetTownQuery, List<GetTownQueryResult>>
    {
        private readonly IRepository<Town> _repository;

        public GetDistrictQueryHandler(IRepository<Town> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetTownQueryResult>> Handle(GetTownQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetTownQueryResult
            {
                TownId = x.TownId,
                TownName = x.TownName,
                CityId = x.CityId
            }).ToList();
        }
    }
}
