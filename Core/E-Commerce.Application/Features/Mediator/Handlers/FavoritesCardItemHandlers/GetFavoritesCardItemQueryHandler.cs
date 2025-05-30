using E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries;
using E_Commerce.Application.Features.Mediator.Queries.FavoritesQueries;
using E_Commerce.Application.Features.Mediator.Results.FavoritesCardItemResults;
using E_Commerce.Application.Features.Mediator.Results.FavoritesCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesCardItemHandlers
{
    public class GetFavoritesCardItemQueryHandler : IRequestHandler<GetFavoritesCardItemQuery, List<GetFavoritesCardItemQueryResult>>
    {
        private readonly IRepository<FavoritesCardItem> _repository;

        public GetFavoritesCardItemQueryHandler(IRepository<FavoritesCardItem> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetFavoritesCardItemQueryResult>> Handle(GetFavoritesCardItemQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetFavoritesCardItemQueryResult
            {
                FavoritesCardItemID = x.FavoritesCardItemID,
                FavoritesCardID = x.FavoritesCardID,
                ProductID = x.ProductID
            }).ToList();
        }
    }
}
