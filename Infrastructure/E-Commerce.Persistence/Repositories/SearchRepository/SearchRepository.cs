using E_Commerce.Application.Interfaces.IProductRepository;
using E_Commerce.Application.Interfaces.ISearchRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.ProductDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.SearchRepository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ECommerceContext _context;

        public SearchRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm, int sayi)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm) ||
                            (p.Category != null && p.Category.Name.Contains(searchTerm)))
                .Take(sayi) // Limit to 10 results
                .ToListAsync();
        }

        public async Task<List<Category>> SearchCategoriesAsync(string searchTerm, int sayi)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(searchTerm) ||
                           c.Description.Contains(searchTerm))
                .Take(sayi) // Limit to 10 results
                .ToListAsync();
        }

        public async Task<List<GetProductDto>> SearchProductsAsyncAll(string searchTerm, string userId = null)
        {
            // İlk olarak searchTerm'e göre ürünleri çekiyoruz
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm) ||
                            (p.Category != null && p.Category.Name.Contains(searchTerm)))
                .ToListAsync();

            var result = new List<GetProductDto>();

            // Eğer userId null değilse, kullanıcının favori kartını buluyoruz
            int? favoritesCardId = null;
            List<FavoritesCardItem> favoritesCardItems = new List<FavoritesCardItem>();

            if (!string.IsNullOrEmpty(userId))
            {
                var favoritesCard = await _context.FavoritesCards
                    .FirstOrDefaultAsync(fc => fc.UserId == int.Parse(userId));

                if (favoritesCard != null)
                {
                    favoritesCardId = favoritesCard.FavoritesCardID;

                    // Bu favori karta ait tüm öğeleri çekiyoruz
                    favoritesCardItems = await _context.FavoritesCardItems
                        .Where(fci => fci.FavoritesCardID == favoritesCardId)
                        .ToListAsync();
                }
            }

            // Her ürün için işlem yapıyoruz
            foreach (var product in products)
            {
                // Bu ürün için toplam sipariş miktarını hesaplıyoruz
                var totalOrder = await _context.OrderItems
                    .Where(oi => oi.ProductID == product.ProductID)
                    .SumAsync(oi => (int?)oi.Quantity) ?? 0;

                // Favori durumunu kontrol ediyoruz
                var favoriteItem = favoritesCardItems.FirstOrDefault(fci => fci.ProductID == product.ProductID);

                var productDto = new GetProductDto
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    Stock = product.Stock,
                    TotalOrder = totalOrder,
                    DiscountRate = product.DiscountRate,
                    ImageUrl = product.ImageUrl,
                    CategoryID = product.CategoryID,
                    UserId = userId?.ToString(),
                    SaleSeason = product.SaleSeason,
                    CategoryName = product.Category?.Name,

                    // Favori bilgileri
                    FavoritesCardID = favoriteItem != null ? favoritesCardId : null,
                    FavoritesCardItemID = favoriteItem?.FavoritesCardItemID,
                    Status = favoriteItem != null // Favorilerde varsa true, yoksa false
                };

                result.Add(productDto);
            }

            return result;
        }

        public async Task<List<Category>> SearchCategoriesAsyncAll(string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(searchTerm) ||
                           c.Description.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
