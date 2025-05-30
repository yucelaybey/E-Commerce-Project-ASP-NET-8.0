using E_Commerce.Application.Interfaces.IFavoritesCardItemRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.FavoritesCardItemDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.FavoritesCardItemRepository
{
    public class FavoritesCardItemRepository : IFavoritesCardItemRepository
    {
        private readonly ECommerceContext _context;

        public FavoritesCardItemRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<FavoritesCardItem>> GetByFavoritesCardIdWithProductsAsync(int favoritesCardId)
        {
            return await _context.FavoritesCardItems
                .Include(fci => fci.Product)
                .Where(fci => fci.FavoritesCardID == favoritesCardId)
                .ToListAsync();
        }
        public async Task<FavoritesCardItem> AddAsync(FavoritesCardItem item)
        {
            await _context.FavoritesCardItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item; // Eklenen entity'yi geri döndürüyoruz (ID dahil)
        }
        public async Task<FavoriteCheckResultDto> CheckFavoriteItemExistsAsync(int? favoritesCardId, int? productId)
        {
            var item = await _context.FavoritesCardItems
                .FirstOrDefaultAsync(x => x.FavoritesCardID == favoritesCardId && x.ProductID == productId);

            return new FavoriteCheckResultDto
            {
                Status = item != null,
                FavoritesCardItemID = item?.FavoritesCardItemID ?? 0,
                FavoritesCardID = item?.FavoritesCardID,
                ProductID = item?.ProductID
            };
        }
    }
}
