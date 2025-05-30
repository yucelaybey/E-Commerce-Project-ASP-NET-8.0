using ECommerce.Dto.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.SearchQueries
{
    public class SearchProductsQuery : IRequest<List<GetProductDto>>
    {
        public string SearchTerm { get; set; }
    }
}
