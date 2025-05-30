using E_Commerce.Application.Features.Mediator.Queries.QuarterQueries;
using E_Commerce.Application.Features.Mediator.Results.QuarterResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.QuarterHandlers
{
    public class GetQuarterQueryHandler : IRequestHandler<GetQuarterQuery, List<GetQuarterQueryResult>>
    {
        private readonly IRepository<Quarter> _repository;

        public GetQuarterQueryHandler(IRepository<Quarter> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetQuarterQueryResult>> Handle(GetQuarterQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetQuarterQueryResult
            {
                QuarterId = x.QuarterId,
                QuarterName = x.QuarterName,
                TownId = x.TownId,
                CityId = x.CityId
            }).ToList();
        }
    }
}
