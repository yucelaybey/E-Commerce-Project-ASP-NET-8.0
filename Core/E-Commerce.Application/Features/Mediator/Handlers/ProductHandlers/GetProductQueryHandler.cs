using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ProductHandlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, List<GetProductQueryResult>>
    {
        private readonly IRepository<Product> _repository;

        public GetProductQueryHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetProductQueryResult>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetProductQueryResult
            {
                ProductID = x.ProductID,
                CategoryID = x.CategoryID,
                Description = x.Description,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Stock = x.Stock,
                SalePrice = x.SalePrice,
                SaleSeason = x.SaleSeason,
                DiscountRate = x.DiscountRate
            }).ToList();
        }
    }
}
