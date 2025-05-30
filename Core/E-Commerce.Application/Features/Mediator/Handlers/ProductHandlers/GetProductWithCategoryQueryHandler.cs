using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using E_Commerce.Application.Interfaces.IProductCategoryDetailRepository;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ProductHandlers
{
    public class GetProductWithCategoryQueryHandler : IRequestHandler<GetProductWithCategoryQuery, GetProductWithCategoryQueryResult>
    {
        private readonly IProductCategoryDetailRepository _repository;

        public GetProductWithCategoryQueryHandler(IProductCategoryDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductWithCategoryQueryResult> Handle(GetProductWithCategoryQuery request, CancellationToken cancellationToken)
        {
            var productWithCategory = await _repository.GetProductWithCategoryAsync(request.Id);

            if (productWithCategory == null)
            {
                return null;
            }

            return new GetProductWithCategoryQueryResult
            {
                ProductID = productWithCategory.ProductID,
                Name = productWithCategory.Name,
                Description = productWithCategory.Description,
                ImageUrl = productWithCategory.ImageUrl,
                Price = productWithCategory.Price,
                SalePrice = productWithCategory.SalePrice,
                DiscountRate = productWithCategory.DiscountRate,
                UserId = productWithCategory.UserId,
                SaleSeason = productWithCategory.SaleSeason,
                Stock = productWithCategory.Stock,
                CategoryID = productWithCategory.CategoryID,
                CategoryName = productWithCategory.CategoryName
            };
        }
    }
}