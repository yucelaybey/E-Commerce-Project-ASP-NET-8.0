using E_Commerce.Domain.Entities;
using ECommerce.Dto.ProductDtos;
using ECommerce.Dto.ShoppingCardItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories
{
    public interface IShoppingCardGetUserIdRepository
    {
        Task<(bool Exists, string ShoppingCardId)> GetShoppingCardUserId(string userId);
        Task<int?> GetTotalQuantityByUserIdAsync(string userId);
        Task<ShoppingCartWithTotalsDto> GetUserCartItemsWithTotalsAsync(string userId);
        Task UpdateQuantityAsync(int shoppingCartItemId, int newQuantity);
        Task<ShoppingCardItem> GetByIdAsync(int id);
        Task UpdateAsync(ShoppingCardItem item);
    }
}
