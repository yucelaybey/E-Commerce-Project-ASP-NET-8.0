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
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, GetCityByIdQueryResult>
    {
        private readonly IRepository<City> _repository;

        public GetCityByIdQueryHandler(IRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task<GetCityByIdQueryResult> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetCityByIdQueryResult
            {
               CityId = values.CityId,
               CityName = values.CityName
            };
        }
    }
}
