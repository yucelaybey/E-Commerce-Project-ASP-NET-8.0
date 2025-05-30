using E_Commerce.Application.Features.Mediator.Queries.FavoritesQueries;
using E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries;
using E_Commerce.Application.Features.Mediator.Results.FavoritesCardResults;
using E_Commerce.Application.Features.Mediator.Results.PaymentCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesHandlers
{
    public class GetFavoritesCardQueryHandler : IRequestHandler<GetFavoritesCardQuery, List<GetFavoritesCardQueryResult>>
    {
        private readonly IRepository<FavoritesCard> _repository;

        public GetFavoritesCardQueryHandler(IRepository<FavoritesCard> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetFavoritesCardQueryResult>> Handle(GetFavoritesCardQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetFavoritesCardQueryResult
            {
                FavoritesCardID = x.FavoritesCardID,
                UserId = x.UserId
            }).ToList();
        }
    }
}
