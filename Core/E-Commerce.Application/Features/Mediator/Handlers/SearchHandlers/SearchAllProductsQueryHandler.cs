
using E_Commerce.Application.Features.Mediator.Queries.SearchQueries;
using E_Commerce.Application.Interfaces.ISearchRepository;
using E_Commerce.Domain.Entities;
using ECommerce.Dto.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.SearchHandlers
{
    public class SearchAllProductsQueryHandler : IRequestHandler<SearchProductAllQuery, List<GetProductDto>>
    {
        private readonly ISearchRepository _productRepository;

        public SearchAllProductsQueryHandler(ISearchRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<GetProductDto>> Handle(SearchProductAllQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.SearchProductsAsyncAll(request.SearchTerm, request.UserId);
        }
    }
}
