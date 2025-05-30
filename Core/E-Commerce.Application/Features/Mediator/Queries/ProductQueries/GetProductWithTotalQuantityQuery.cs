using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.ProductQueries
{
    public class GetProductWithTotalQuantityQuery : IRequest<List<GetProductWithTotalQuantityResult>>
    {
        public int? FavoritesCardId { get; set; }
    }
}
