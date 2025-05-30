using E_Commerce.Application.Interfaces.IThirtyLastOrderItemTotalAmountRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories.ThirtyLastOrderItemTotalAmountRepository
{
    public class ThirtyLastOrderItemTotalAmountRepository : IThirtyLastOrderItemTotalAmountRepository
    {
        private readonly ECommerceContext _context;

        public ThirtyLastOrderItemTotalAmountRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetOrderItemListLastThirtyOrderTotalAmount()
        {
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var orders = await _context.OrderItems
                                      .Where(o => o.OrderDate >= thirtyDaysAgo)
                                      .ToListAsync();

            return orders;
        }
    }
}
