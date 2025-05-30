using ECommerce.Dto.OrderItemDtos;

namespace E_Commerce.Application.Interfaces.IBestSellerProductAndAmountRepository
{
    public interface IBestSellerProductAndAmountRepository
    {
        Task<List<BestSellerProductDto>> GetBestSellerProducts(int? favoritesCardId = null);
    }
}
