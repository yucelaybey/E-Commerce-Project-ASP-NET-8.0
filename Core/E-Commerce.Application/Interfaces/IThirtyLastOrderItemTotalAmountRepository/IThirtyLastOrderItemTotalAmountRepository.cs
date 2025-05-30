using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.IThirtyLastOrderItemTotalAmountRepository
{
    public interface IThirtyLastOrderItemTotalAmountRepository
    {
        Task<List<OrderItem>> GetOrderItemListLastThirtyOrderTotalAmount();
    }
}
