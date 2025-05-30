using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IOrderRepository
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
    }
}
