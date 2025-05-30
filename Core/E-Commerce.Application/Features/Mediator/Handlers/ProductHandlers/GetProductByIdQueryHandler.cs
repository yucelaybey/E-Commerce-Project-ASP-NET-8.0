using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ProductHandlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResult>
    {
        private readonly IRepository<Product> _repository;

        public GetProductByIdQueryHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetProductByIdQueryResult
            {
                ProductID = values.ProductID,
                CategoryID = values.CategoryID,
                Name = values.Name,
                Description = values.Description,
                ImageUrl = values.ImageUrl,
                Price = values.Price,
                SalePrice = values.SalePrice,
                DiscountRate = values.DiscountRate,
                SaleSeason = values.SaleSeason,
                Stock = values.Stock

            };
        }
    }
}
