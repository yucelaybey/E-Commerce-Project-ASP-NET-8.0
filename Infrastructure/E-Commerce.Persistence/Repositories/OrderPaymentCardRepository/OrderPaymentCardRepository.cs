using E_Commerce.Application.Interfaces.IOrderPaymentCardRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.OrderPaymentCardRepository
{
    public class OrderPaymentCardRepository : IOrderPaymentCardRepository
    {
        private readonly ECommerceContext _context;

        public OrderPaymentCardRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderPaymentCardAsync(OrderPaymentCard order)
        {
            _context.OrderPaymentCards.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderPaymentCardId;
        }
    }
}
