using ECommerce.Dto.ProductDtos;

namespace E_Commerce.Application.Interfaces.IProductCategoryDetailRepository
{
    public interface IProductCategoryDetailRepository
    {
        Task<ProductWithCategoryDto> GetProductWithCategoryAsync(int productId);
    }
}
