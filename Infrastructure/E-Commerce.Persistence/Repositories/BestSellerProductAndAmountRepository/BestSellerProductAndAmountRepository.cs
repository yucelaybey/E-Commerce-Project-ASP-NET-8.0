using E_Commerce.Application.Interfaces.IBestSellerProductAndAmountRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.OrderItemDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class BestSellerProductAndAmountRepository : IBestSellerProductAndAmountRepository
    {
        private readonly ECommerceContext _context;
        private readonly ILogger<BestSellerProductAndAmountRepository> _logger;

        public BestSellerProductAndAmountRepository(
            ECommerceContext context,
            ILogger<BestSellerProductAndAmountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<BestSellerProductDto>> GetBestSellerProducts(int? favoritesCardId = null)
        {
            // Get best selling products first
            var bestSellerProducts = await GetBestSellingProducts();

            // If no favoritesCardId provided, return without favorite info
            if (!favoritesCardId.HasValue)
                return bestSellerProducts;

            // Get favorite info and join in memory
            var favoriteInfo = await GetFavoriteProductInfo(favoritesCardId.Value);
            return JoinWithFavoriteInfo(bestSellerProducts, favoriteInfo, favoritesCardId.Value);
        }

        private async Task<List<BestSellerProductDto>> GetBestSellingProducts()
        {
            return await _context.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .GroupBy(oi => oi.ProductID)
                .Select(g => new BestSellerProductDto
                {
                    ProductID = g.Key,
                    Name = g.First().Product.Name,
                    Description = g.First().Product.Description,
                    Price = g.First().Product.Price,
                    SalePrice = g.First().Product.SalePrice,
                    Stock = g.First().Product.Stock,
                    DiscountRate = g.First().Product.DiscountRate,
                    ImageUrl = g.First().Product.ImageUrl,
                    CategoryID = g.First().Product.CategoryID,
                    CategoryName = g.First().Product.Category.Name,
                    SaleSeason = g.First().Product.SaleSeason,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    Favorites = false, // Default to false, will update later if needed
                    FavoritesCardID = null,
                    FavoritesCardItemID = null
                })
                .OrderByDescending(p => p.TotalQuantity)
                .ToListAsync();
        }

        private async Task<(HashSet<int> FavoriteProductIds, Dictionary<int, int> ProductIdToFavoritesItemId)>
            GetFavoriteProductInfo(int favoritesCardId)
        {
            var favoritesCardExists = await _context.FavoritesCards
                .AsNoTracking()
                .AnyAsync(f => f.FavoritesCardID == favoritesCardId);

            if (!favoritesCardExists)
                return (null, null);

            var favoriteItems = await _context.FavoritesCardItems
                .AsNoTracking()
                .Where(f => f.FavoritesCardID == favoritesCardId)
                .Select(f => new { f.ProductID, f.FavoritesCardItemID })
                .ToListAsync();

            return (
                new HashSet<int>(favoriteItems.Select(x => x.ProductID)),
                favoriteItems.ToDictionary(x => x.ProductID, x => x.FavoritesCardItemID)
            );
        }

        private List<BestSellerProductDto> JoinWithFavoriteInfo(
            List<BestSellerProductDto> products,
            (HashSet<int> FavoriteProductIds, Dictionary<int, int> ProductIdToFavoritesItemId) favoriteInfo,
    int favoritesCardId)
        {
            if (favoriteInfo.FavoriteProductIds == null)
                return products;

            foreach (var product in products)
            {
                product.FavoritesCardID = favoritesCardId;

                if (favoriteInfo.FavoriteProductIds.Contains(product.ProductID))
                {
                    product.Favorites = true;
                    product.FavoritesCardItemID = favoriteInfo.ProductIdToFavoritesItemId[product.ProductID];
                }
            }

            return products;
        }
    }
}