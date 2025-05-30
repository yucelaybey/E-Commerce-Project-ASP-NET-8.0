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
    public class GetFavoritesCardItemByIdQueryHandler : IRequestHandler<GetFavoritesCardItemByIdQuery, GetFavoritesCardItemByIdQueryResult>
    {
        private readonly IRepository<FavoritesCardItem> _repository;

        public GetFavoritesCardItemByIdQueryHandler(IRepository<FavoritesCardItem> repository)
        {
            _repository = repository;
        }

        public async Task<GetFavoritesCardItemByIdQueryResult> Handle(GetFavoritesCardItemByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetFavoritesCardItemByIdQueryResult
            {
                FavoritesCardItemID = values?.FavoritesCardItemID,
                FavoritesCardID = values?.FavoritesCardID,
                ProductID = values?.ProductID
            };
        }
    }
}
