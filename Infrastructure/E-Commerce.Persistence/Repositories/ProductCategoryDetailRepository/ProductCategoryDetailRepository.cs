using E_Commerce.Application.Interfaces.IProductCategoryDetailRepository;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.ProductDtos;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories.ProductCategoryDetailRepository
{
    public class ProductCategoryDetailRepository : IProductCategoryDetailRepository
    {
        private readonly ECommerceContext _context;

        public ProductCategoryDetailRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<ProductWithCategoryDto> GetProductWithCategoryAsync(int productId)
        {
            var productWithCategory = await (from product in _context.Products
                                             join category in _context.Categories
                                             on product.CategoryID equals category.CategoryID
                                             where product.ProductID == productId
                                             select new ProductWithCategoryDto
                                             {
                                                 ProductID = product.ProductID,
                                                 Name = product.Name,
                                                 Description = product.Description,
                                                 ImageUrl = product.ImageUrl,
                                                 Price = product.Price,
                                                 SalePrice = product.SalePrice,
                                                 DiscountRate = product.DiscountRate,
                                                 SaleSeason = product.SaleSeason,
                                                 Stock = product.Stock,
                                                 CategoryID = category.CategoryID,
                                                 CategoryName = category.Name
                                             }).FirstOrDefaultAsync();

            return productWithCategory;
        }
    }
}
