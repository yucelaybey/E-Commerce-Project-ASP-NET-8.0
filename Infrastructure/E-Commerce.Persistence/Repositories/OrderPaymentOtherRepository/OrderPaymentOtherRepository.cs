using E_Commerce.Application.Interfaces.IOrderPaymentOtherRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.OrderPaymentOtherRepository
{
    public class OrderPaymentOtherRepository : IOrderPaymentOtherRepository
    {
        private readonly ECommerceContext _context;

        public OrderPaymentOtherRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderPaymentOtherAsync(OrderPaymentOther order)
        {
            _context.OrderPaymentOthers.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderPaymentOtherId;
        }
    }
}
