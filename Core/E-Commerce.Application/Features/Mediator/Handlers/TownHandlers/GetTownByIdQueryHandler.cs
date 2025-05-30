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
    public class GetDistrictByIdQueryHandler : IRequestHandler<GetTownByIdQuery, GetTownByIdQueryResult>
    {
        private readonly IRepository<Town> _repository;

        public GetDistrictByIdQueryHandler(IRepository<Town> repository)
        {
            _repository = repository;
        }

        public async Task<GetTownByIdQueryResult> Handle(GetTownByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetTownByIdQueryResult
            {
               TownId = values.TownId,
               TownName = values.TownName
            };
        }
    }
}
