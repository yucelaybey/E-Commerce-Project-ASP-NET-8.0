using ECommerce.Dto.CategoryDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.SearchQueries
{
    public class SearchCategoriesQuery : IRequest<List<GetCategoryDto>>
    {
        public string SearchTerm { get; set; }
    }
}
