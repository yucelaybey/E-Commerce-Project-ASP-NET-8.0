using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IShoppingCardItemCheckRepository
{
    public interface IShoppingCardItemCheckRepository
    {
        Task<bool> ExistsAsync(int productId, int shoppingCardId);
        Task UpdateQuantityAsync(int productId, int shoppingCardId, int quantityToAdd);

        Task CreateAsync(ShoppingCardItem item);
    }
}
