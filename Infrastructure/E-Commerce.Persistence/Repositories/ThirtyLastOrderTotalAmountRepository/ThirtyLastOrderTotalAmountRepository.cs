using E_Commerce.Application.Interfaces.IThirtyLastOrderTotalAmount;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories.ThirtyLastOrderTotalAmountRepository
{
    public class ThirtyLastOrderTotalAmountRepository : IThirtyLastOrderTotalAmountRepository
    {
        private readonly ECommerceContext _context;

        public ThirtyLastOrderTotalAmountRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrderListLastThirtyOrderTotalAmount()
        {
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var orders = await _context.Orders
                                      .Where(o => o.OrderDate >= thirtyDaysAgo)
                                      .ToListAsync();

            return orders;
        }
    }
}