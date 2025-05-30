// E_Commerce.Infrastructure.Repositories
using E_Commerce.Application.Interfaces.IShoppingCardItemCheckRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class ShoppingCardItemCheckRepository : IShoppingCardItemCheckRepository
    {
        private readonly ECommerceContext _context;

        public ShoppingCardItemCheckRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int productId, int shoppingCardId)
        {
            return await _context.ShoppingCardItems
        .Where(x => x.ShoppingCardID == shoppingCardId)
        .AnyAsync(x => x.ProductID == productId);
        }
        public async Task UpdateQuantityAsync(int productId, int shoppingCardId, int quantityToAdd)
        {
            var item = await _context.ShoppingCardItems
                .Include(x => x.Product) // Product bilgisini include ediyoruz
                .FirstOrDefaultAsync(x => x.ProductID == productId &&
                                        x.ShoppingCardID == shoppingCardId);

            if (item != null)
            {
                item.Quantity += quantityToAdd;

                // SaleSeason'e göre fiyat hesaplaması
                if (item.Product.SaleSeason)
                {
                    item.TotalPrice = item.Quantity * (int)item.Product.SalePrice;
                }
                else
                {
                    item.TotalPrice = item.Quantity * item.Product.Price;
                }

                _context.ShoppingCardItems.Update(item);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CreateAsync(ShoppingCardItem item)
        {
            // Sadece gerekli alanları set edin
            var newItem = new ShoppingCardItem
            {
                ProductID = item.ProductID,
                ShoppingCardID = item.ShoppingCardID,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
                Status =item.Status
            };

            _context.ShoppingCardItems.Add(newItem);
            await _context.SaveChangesAsync();
        }
    }
}