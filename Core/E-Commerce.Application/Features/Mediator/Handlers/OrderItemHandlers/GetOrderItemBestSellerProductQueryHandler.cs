using E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries;
using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using E_Commerce.Application.Interfaces.IBestSellerProductAndAmountRepository;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class GetBestSellerProductsQueryHandler : IRequestHandler<GetProductWithTotalQuantityQuery, List<GetProductWithTotalQuantityResult>>
    {
        private readonly IBestSellerProductAndAmountRepository _repository;

        public GetBestSellerProductsQueryHandler(IBestSellerProductAndAmountRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetProductWithTotalQuantityResult>> Handle(GetProductWithTotalQuantityQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetBestSellerProducts(request.FavoritesCardId);

            return values.Select(x => new GetProductWithTotalQuantityResult
            {
                ProductID = x.ProductID,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                SalePrice = x.SalePrice,
                DiscountRate = x.DiscountRate,
                SaleSeason = x.SaleSeason,
                Stock = x.Stock,
                TotalQuantity = x.TotalQuantity,
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName,
                Favorites = x.Favorites,
                FavoritesCardID =x.FavoritesCardID,
                FavoritesCardItemID =x.FavoritesCardItemID
            }).ToList();
        }
    }
}