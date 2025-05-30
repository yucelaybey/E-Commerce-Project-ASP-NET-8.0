using E_Commerce.Application.Interfaces.IBestFiveShoppingCardAddProductRepository;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.ShoppingCardItem;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories.BestFiveShoppingCardAddProductRepository
{
    public class BestFiveShoppingCardAddProductRepository : IBestFiveShoppingCardAddProductRepository
    {
        private readonly ECommerceContext _context;

        public BestFiveShoppingCardAddProductRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<BestFiveAddShoppingCardItemDto>> GetMostAddedProductsAsync()
        {
            var mostAddedProducts = await _context.ShoppingCardItems
                .Include(sci => sci.Product) // Product bilgilerini join et
                .GroupBy(sci => sci.ProductID) // ProductID'e göre grupla
                .Select(g => new BestFiveAddShoppingCardItemDto
                {
                    ProductName = g.First().Product.Name, // Ürün adı
                    TotalQuantityAdded = g.Sum(sci => sci.Quantity) // Toplam eklenme adedi
                })
                .OrderByDescending(g => g.TotalQuantityAdded) // En çok eklenene göre sırala
                .Take(5) // İlk 5 ürünü al
                .ToListAsync();

            return mostAddedProducts;
        }
    }
}
