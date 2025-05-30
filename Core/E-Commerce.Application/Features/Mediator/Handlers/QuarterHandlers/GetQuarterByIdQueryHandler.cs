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
    public class GetQuarterByIdQueryHandler : IRequestHandler<GetQuarterByIdQuery, GetQuarterByIdQueryResult>
    {
        private readonly IRepository<Quarter> _repository;

        public GetQuarterByIdQueryHandler(IRepository<Quarter> repository)
        {
            _repository = repository;
        }

        public async Task<GetQuarterByIdQueryResult> Handle(GetQuarterByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetQuarterByIdQueryResult
            {
                QuarterId = values.QuarterId,
                QuarterName = values.QuarterName,
                CityId = values.CityId,
                TownId = values.TownId
            };
        }
    }
}
