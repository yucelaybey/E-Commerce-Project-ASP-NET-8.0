using E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderItemResults;
using E_Commerce.Application.Interfaces.IThirtyLastOrderItemTotalAmountRepository;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class GetThirtyLastOrderItemTotalAmountQueryHandler : IRequestHandler<GetOrderItemQuery, List<GetOrderItemQueryResult>>
    {
        private readonly IThirtyLastOrderItemTotalAmountRepository _repository;

        public GetThirtyLastOrderItemTotalAmountQueryHandler(IThirtyLastOrderItemTotalAmountRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderItemQueryResult>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetOrderItemListLastThirtyOrderTotalAmount();
            return values.Select(x => new GetOrderItemQueryResult
            {
                OrderID = x.OrderID
            }).ToList();
        }
    }
}
