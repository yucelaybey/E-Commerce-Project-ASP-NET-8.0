using ECommerce.Dto.ShoppingCardItem;

namespace E_Commerce.Application.Interfaces.IBestFiveShoppingCardAddProductRepository
{
    public interface IBestFiveShoppingCardAddProductRepository
    {
        Task<List<BestFiveAddShoppingCardItemDto>> GetMostAddedProductsAsync();
    }
}
