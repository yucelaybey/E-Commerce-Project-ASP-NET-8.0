using AutoMapper;
using E_Commerce.Application.Features.Mediator.Queries.SearchQueries;
using E_Commerce.Application.Interfaces.IProductRepository;
using E_Commerce.Application.Interfaces.ISearchRepository;
using ECommerce.Dto.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.SearchHandlers
{
    public class SearchProductQueryHandler : IRequestHandler<SearchProductsQuery, List<GetProductDto>>
    {
        private readonly ISearchRepository _productRepository;

        public SearchProductQueryHandler(ISearchRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<GetProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
                return new List<GetProductDto>();


            var products = await _productRepository.SearchProductsAsync(request.SearchTerm, 10);

            return products.Select(p => new GetProductDto
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SalePrice = p.SalePrice,
                Stock = p.Stock,
                DiscountRate = p.DiscountRate,
                ImageUrl = p.ImageUrl,
                CategoryID = p.CategoryID,
                CategoryName = p.Category?.Name,
                SaleSeason = p.SaleSeason,
            }).ToList();
        }
    }
}
