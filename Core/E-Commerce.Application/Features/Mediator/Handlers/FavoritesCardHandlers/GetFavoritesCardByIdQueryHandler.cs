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
    public class GetFavoritesCardByIdQueryHandler : IRequestHandler<GetFavoritesCardByIdQuery, GetFavoritesCardByIdQueryResult>
    {
        private readonly IRepository<FavoritesCard> _repository;

        public GetFavoritesCardByIdQueryHandler(IRepository<FavoritesCard> repository)
        {
            _repository = repository;
        }

        public async Task<GetFavoritesCardByIdQueryResult> Handle(GetFavoritesCardByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetFavoritesCardByIdQueryResult
            {
                FavoritesCardID = values.FavoritesCardID,
                UserId = values.UserId
            };
        }
    }
}
