using E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderItemResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, GetOrderItemByIdQueryResult>
    {
        private readonly IRepository<OrderItem> _repository;

        public GetOrderItemByIdQueryHandler(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderItemByIdQueryResult> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetOrderItemByIdQueryResult
            {
                OrderItemID = values.OrderItemID,
                ProductID = values.ProductID,
                Quantity = values.Quantity,
                Price = values.Price,
                OrderStatus = values.OrderStatus
            };
        }
    }
}
