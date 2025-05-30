using ECommerce.Dto.CategoryProductList;

namespace E_Commerce.Application.Interfaces.ICategoryProductRepository
{
    public interface ICategoryProductRepository
    {
        Task<List<CategoryProductListDto>> GetProductsByCategoryIdWithCartInfoAsync(int categoryId, int? favoritesCardId = null);
    }
}
