using E_Commerce.Domain.Entities;
using ECommerce.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.ISearchRepository
{
    public interface ISearchRepository
    {
        Task<List<Product>> SearchProductsAsync(string searchTerm, int sayi);
        Task<List<Category>> SearchCategoriesAsync(string searchTerm, int sayi);
        Task<List<GetProductDto>> SearchProductsAsyncAll(string searchTerm, string userId = null);
        Task<List<Category>> SearchCategoriesAsyncAll(string searchTerm);

    }
}
