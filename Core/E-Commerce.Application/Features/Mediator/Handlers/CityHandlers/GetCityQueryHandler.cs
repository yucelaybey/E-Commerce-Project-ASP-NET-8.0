using E_Commerce.Application.Features.Mediator.Queries.CityQueries;
using E_Commerce.Application.Features.Mediator.Results.CityResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.CityHandlers
{
    public class GetCityQueryHandler : IRequestHandler<GetCityQuery, List<GetCityQueryResult>>
    {
        private readonly IRepository<City> _repository;

        public GetCityQueryHandler(IRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetCityQueryResult>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetCityQueryResult
            {
                CityId = x.CityId,
                CityName = x.CityName
            }).ToList();
        }
    }
}
