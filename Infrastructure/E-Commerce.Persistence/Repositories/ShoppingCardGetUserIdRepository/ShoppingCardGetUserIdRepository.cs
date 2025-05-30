using E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.ShoppingCardItemDtos;
using Microsoft.EntityFrameworkCore;

public class ShoppingCardGetUserIdRepository : IShoppingCardGetUserIdRepository
{
    private readonly ECommerceContext _context;

    public ShoppingCardGetUserIdRepository(ECommerceContext context)
    {
        _context = context;
    }

    public async Task<(bool Exists, string ShoppingCardId)> GetShoppingCardUserId(string userId)
    {
        var shoppingCard = await _context.ShoppingCards
            .FirstOrDefaultAsync(sc => sc.UserId == userId);

        return (shoppingCard != null, shoppingCard?.ShoppingCardID.ToString());
    }
    public async Task<int?> GetTotalQuantityByUserIdAsync(string userId)
    {
        // 1. UserId'den ShoppingCardID'yi bul
        var shoppingCardId = await _context.ShoppingCards
            .Where(sc => sc.UserId == userId)
            .Select(sc => (int?)sc.ShoppingCardID)
            .FirstOrDefaultAsync();

        if (shoppingCardId == null) return null;

        // 2. ShoppingCardID'ye göre Quantity topla
        return await _context.ShoppingCardItems
            .Where(item => item.ShoppingCardID == shoppingCardId)
            .SumAsync(item => (int?)item.Quantity);
    }

    public async Task<ShoppingCartWithTotalsDto> GetUserCartItemsWithTotalsAsync(string userId)
    {
        var result = new ShoppingCartWithTotalsDto();

        // 1. UserId ile ShoppingCard'ı bul
        var shoppingCart = await _context.ShoppingCards
            .FirstOrDefaultAsync(sc => sc.UserId == userId);

        if (shoppingCart == null)
            return result; // Boş sepet döndür

        // 2. ShoppingCardItem'ları ve Product bilgilerini getir
        var items = await _context.ShoppingCardItems
            .Include(item => item.Product)
            .Where(item => item.ShoppingCardID == shoppingCart.ShoppingCardID)
            .Select(item => new ShoppingCartItemDto
            {
                ShoppingCartItemID = item.ShoppingCardItemID,
                ShoppingCartID = item.ShoppingCardID,
                ProductId = item.Product.ProductID,
                ProductName = item.Product.Name,
                ProductDescription = item.Product.Description,
                ProductImageUrl = item.Product.ImageUrl,
                ProductPrice = item.Product.Price,
                ProductDiscountRate = item.Product.DiscountRate,
                ProductSaleSeason = item.Product.SaleSeason,
                Quantity = item.Quantity,
                ProductSalePrice = item.Product.SalePrice,
                ProductStock = item.Product.Stock,
                Status = item.Status
            })
            .ToListAsync();

        // 3. Sepet genel toplamlarını hesapla
        result.TotalStatus = items.All(x => x.Status);
        result.TrueStatusCount = items.Sum(x => x.Status ? 1 : 0);
        result.Items = items;
        result.TotalPrice = items.Sum(item => item.ItemTotalPrice);

        // TotalSalePrice hesaplaması (indirim sezonu olanların indirimli, olmayanların normal fiyatı)
        result.TotalSalePrice = items.Sum(item =>
            item.ProductSaleSeason ? item.ItemTotalSalePrice : item.ItemTotalPrice);

        result.TotalDiscountAmount = result.TotalPrice - result.TotalSalePrice;
        result.TotalDiscountRate = result.TotalPrice > 0
            ? Math.Round((result.TotalDiscountAmount / result.TotalPrice) * 100, 2)
            : 0;

        return result;
    }

    public async Task UpdateQuantityAsync(int shoppingCartItemId, int newQuantity)
    {
        var item = await _context.ShoppingCardItems
            .FindAsync(shoppingCartItemId);

        if (item == null)
            throw new Exception("Sepet öğesi bulunamadı");

        item.Quantity = newQuantity;
        await _context.SaveChangesAsync();
    }

    public async Task<ShoppingCardItem> GetByIdAsync(int id)
    {
        return await _context.ShoppingCardItems.FindAsync(id);
    }

    public async Task UpdateAsync(ShoppingCardItem item)
    {
        _context.ShoppingCardItems.Update(item);
        await _context.SaveChangesAsync();
    }
}