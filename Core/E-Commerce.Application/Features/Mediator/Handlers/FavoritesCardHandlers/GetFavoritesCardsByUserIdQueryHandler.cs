using E_Commerce.Application.Features.Mediator.Queries.FavoritesCardQueries;
using E_Commerce.Application.Interfaces.IFavoritesCardRepository;
using ECommerce.Dto.FavoritesCardDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesCardHandlers
{
    public class GetFavoritesCardsByUserIdQueryHandler : IRequestHandler<GetFavoritesCardsByUserIdQuery, List<FavoritesCardDto>>
    {
        private readonly IFavoritesCardRepository _repository;

        public GetFavoritesCardsByUserIdQueryHandler(IFavoritesCardRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FavoritesCardDto>> Handle(GetFavoritesCardsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var favoritesCards = await _repository.GetByUserIdAsync(request.UserId);

            return favoritesCards.Select(fc => new FavoritesCardDto
            {
                favoritesCardId = fc.FavoritesCardID
            }).ToList();
        }
    }
}
