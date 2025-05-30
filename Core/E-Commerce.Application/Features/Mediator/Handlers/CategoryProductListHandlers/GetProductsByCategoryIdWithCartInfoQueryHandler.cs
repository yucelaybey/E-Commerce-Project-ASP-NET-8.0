using E_Commerce.Application.Features.Mediator.Queries.CategoryProductListQueries;
using E_Commerce.Application.Interfaces.ICategoryProductRepository;
using ECommerce.Dto.CategoryProductList;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.CategoryProductListHandlers
{
    public class GetProductsByCategoryIdWithCartInfoQueryHandler : IRequestHandler<GetProductsByCategoryIdWithCartInfoQuery, List<CategoryProductListDto>>
    {
        private readonly ICategoryProductRepository _category;

        public GetProductsByCategoryIdWithCartInfoQueryHandler(ICategoryProductRepository category)
        {
            _category = category;
        }

        public async Task<List<CategoryProductListDto>> Handle(GetProductsByCategoryIdWithCartInfoQuery request, CancellationToken cancellationToken)
        {
            return await _category.GetProductsByCategoryIdWithCartInfoAsync(request.CategoryId, request.FavoritesCardId);
        }
    }
}
