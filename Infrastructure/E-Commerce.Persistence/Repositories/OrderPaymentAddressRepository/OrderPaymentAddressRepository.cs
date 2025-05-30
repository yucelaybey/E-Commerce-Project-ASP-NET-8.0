using E_Commerce.Application.Interfaces.IOrderPaymentAddressRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.OrderPaymentAddressRepository
{
    public class OrderPaymentAddressRepository:IOrderPaymentAddressRepository
    {
        private readonly ECommerceContext _context;

        public OrderPaymentAddressRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderPaymentAddressAsync(OrderPaymentAddress order)
        {
            _context.OrderPaymentAddresses.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderPaymentAddressId;
        }
    }
}
