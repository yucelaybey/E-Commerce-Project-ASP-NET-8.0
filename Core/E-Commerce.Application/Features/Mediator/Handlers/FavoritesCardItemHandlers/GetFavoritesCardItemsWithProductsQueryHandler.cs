using E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries;
using E_Commerce.Application.Interfaces.IFavoritesCardItemRepository;
using ECommerce.Dto.FavoritesCardItemDtos;
using ECommerce.Dto.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesCardItemHandlers
{
    public class GetFavoritesCardItemsWithProductsQueryHandler
    : IRequestHandler<GetFavoritesCardItemsWithProductsQuery, List<FavoritesCardItemWithProductDto>>
    {
        private readonly IFavoritesCardItemRepository _repository;

        public GetFavoritesCardItemsWithProductsQueryHandler(IFavoritesCardItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FavoritesCardItemWithProductDto>> Handle(
            GetFavoritesCardItemsWithProductsQuery request,
            CancellationToken cancellationToken)
        {
            var items = await _repository.GetByFavoritesCardIdWithProductsAsync(request.FavoritesCardId);

            return items.Select(item => new FavoritesCardItemWithProductDto
            {
                FavoritesCardItemId = item.FavoritesCardItemID,
                FavoritesCardId = item.FavoritesCardID,
                Product = new GetProductDto
                {
                    ProductID = item.Product.ProductID,
                    Name = item.Product.Name,
                    Description = item.Product.Description,
                    Price = item.Product.Price,
                    SalePrice = item.Product.SalePrice,
                    Stock = item.Product.Stock,
                    DiscountRate = item.Product.DiscountRate,
                    ImageUrl = item.Product.ImageUrl,
                    CategoryID = item.Product.CategoryID,
                    SaleSeason = item.Product.SaleSeason
                }
            }).ToList();
        }
    }
}
