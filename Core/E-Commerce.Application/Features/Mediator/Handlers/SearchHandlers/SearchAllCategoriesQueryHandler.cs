using E_Commerce.Application.Features.Mediator.Queries.SearchQueries;
using E_Commerce.Application.Interfaces.ISearchRepository;
using ECommerce.Dto.CategoryDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.SearchHandlers
{
    public class SearchAllCategoriesQueryHandler : IRequestHandler<SearchCategoriesQuery, List<GetCategoryDto>>
    {
        private readonly ISearchRepository _categoryRepository;

        public SearchAllCategoriesQueryHandler(ISearchRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetCategoryDto>> Handle(SearchCategoriesQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
                return new List<GetCategoryDto>();

            var categories = await _categoryRepository.SearchCategoriesAsyncAll(request.SearchTerm);

            return categories.Select(c => new GetCategoryDto
            {
                CategoryID = c.CategoryID,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl
            }).ToList();
        }
    }
}
