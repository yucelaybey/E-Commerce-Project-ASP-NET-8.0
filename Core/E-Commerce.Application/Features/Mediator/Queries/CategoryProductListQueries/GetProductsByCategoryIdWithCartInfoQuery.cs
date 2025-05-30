using ECommerce.Dto.CategoryProductList;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.CategoryProductListQueries
{
    public class GetProductsByCategoryIdWithCartInfoQuery : IRequest<List<CategoryProductListDto>>
    {
        public int CategoryId { get; set; }
        public int? FavoritesCardId { get; set; }
    }
}
