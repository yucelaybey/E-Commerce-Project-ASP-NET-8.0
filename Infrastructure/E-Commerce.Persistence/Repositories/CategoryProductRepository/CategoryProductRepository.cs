using E_Commerce.Application.Interfaces.ICategoryProductRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.CategoryProductList;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories.CategoryProductRepository
{
    public class CategoryProductRepository : ICategoryProductRepository
    {
        private readonly ECommerceContext _context;

        public CategoryProductRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryProductListDto>> GetProductsByCategoryIdWithCartInfoAsync(int categoryId, int? favoritesCardId = null)
        {
            // Tüm ürünleri ve ilişkili OrderItems'ları alıyoruz
            var products = await _context.Products
                .Where(p => p.CategoryID == categoryId)
                .Include(p => p.Category)
                .Include(p => p.OrderItems) // OrderItems'ı include ediyoruz
                .ToListAsync();

            // Favori bilgilerini alıyoruz
            Dictionary<int, int> favoritesDict = new();
            if (favoritesCardId.HasValue)
            {
                favoritesDict = await _context.FavoritesCardItems
                    .Where(fci => fci.FavoritesCardID == favoritesCardId)
                    .ToDictionaryAsync(fci => fci.ProductID, fci => fci.FavoritesCardItemID);
            }

            // Sonucu oluşturuyoruz
            var result = products.Select(p => new CategoryProductListDto
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SalePrice = p.SalePrice,
                Stock = p.Stock,
                DiscountRate = p.DiscountRate,
                ImageUrl = p.ImageUrl,
                CategoryID = p.CategoryID,
                CategoryName = p.Category?.Name,
                SaleSeason = p.SaleSeason,
                // OrderItems'daki quantity'leri topluyoruz
                TotalQuantityInCart = p.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                Favorites = favoritesCardId.HasValue && favoritesDict.ContainsKey(p.ProductID),
                FavoritesCardItemID = favoritesCardId.HasValue && favoritesDict.TryGetValue(p.ProductID, out var favId)
                    ? favId
                    : null,
                FavoritesCardID = favoritesCardId.HasValue && favoritesDict.ContainsKey(p.ProductID)
                    ? favoritesCardId
                    : null
            }).ToList();

            return result;
        }
    }
}
