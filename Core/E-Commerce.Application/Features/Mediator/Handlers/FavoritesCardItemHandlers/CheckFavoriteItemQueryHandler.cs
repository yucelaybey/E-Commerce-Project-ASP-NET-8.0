using E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries;
using E_Commerce.Application.Interfaces.IFavoritesCardItemRepository;
using ECommerce.Dto.FavoritesCardItemDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesCardItemHandlers
{
    public class CheckFavoriteItemQueryHandler : IRequestHandler<CheckFavoriteItemQuery, FavoriteCheckResultDto>
    {
        private readonly IFavoritesCardItemRepository _favoritesRepository;

        public CheckFavoriteItemQueryHandler(IFavoritesCardItemRepository favoritesRepository)
        {
            _favoritesRepository = favoritesRepository;
        }

        public async Task<FavoriteCheckResultDto> Handle(CheckFavoriteItemQuery request, CancellationToken cancellationToken)
        {
            return await _favoritesRepository.CheckFavoriteItemExistsAsync(
                request.FavoritesCardID,
                request.ProductID);
        }
    }
}
