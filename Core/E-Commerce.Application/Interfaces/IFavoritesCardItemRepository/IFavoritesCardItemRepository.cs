using E_Commerce.Domain.Entities;
using ECommerce.Dto.FavoritesCardItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IFavoritesCardItemRepository
{
    public interface IFavoritesCardItemRepository
    {
        Task<List<FavoritesCardItem>> GetByFavoritesCardIdWithProductsAsync(int favoritesCardId);
        Task<FavoritesCardItem> AddAsync(FavoritesCardItem item);
        Task<FavoriteCheckResultDto> CheckFavoriteItemExistsAsync(int? favoritesCardId, int? productId);
    }
}
