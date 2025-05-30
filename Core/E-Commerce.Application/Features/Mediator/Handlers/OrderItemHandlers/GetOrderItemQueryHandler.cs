using E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderItemResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, List<GetOrderItemQueryResult>>
    {
        private readonly IRepository<OrderItem> _repository;

        public GetOrderItemQueryHandler(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderItemQueryResult>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetOrderItemQueryResult
            {
                OrderItemID = x.OrderItemID,
                OrderID = x.OrderID,
                ProductID = x.ProductID,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                DiscountRate = x.DiscountRate,
                CategoryID = x.CategoryID,
                SaleSeason = x.SaleSeason,
                SalePrice = x.SalePrice,
                Stock = x.Stock,
                Quantity = x.Quantity,
                Price = x.Quantity,
                OrderStatus = x.OrderStatus,
                OrderDate = x.OrderDate
            }).ToList();
        }
    }
}
