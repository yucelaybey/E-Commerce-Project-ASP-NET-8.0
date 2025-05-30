using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.IThirtyLastOrderTotalAmount
{
    public interface IThirtyLastOrderTotalAmountRepository
    {
        Task<List<Order>> GetOrderListLastThirtyOrderTotalAmount();
    }
}
