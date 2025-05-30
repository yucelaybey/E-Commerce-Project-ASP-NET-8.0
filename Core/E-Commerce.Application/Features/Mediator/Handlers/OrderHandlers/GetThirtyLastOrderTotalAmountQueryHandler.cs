using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using E_Commerce.Application.Interfaces.IThirtyLastOrderTotalAmount;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class GetThirtyLastOrderTotalAmountQueryHandler : IRequestHandler<GetThirtyLastOrderTotalAmountQuery, List<GetThirtyLastOrderTotalAmountQueryResult>>
    {
        private readonly IThirtyLastOrderTotalAmountRepository _repository;

        public GetThirtyLastOrderTotalAmountQueryHandler(IThirtyLastOrderTotalAmountRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetThirtyLastOrderTotalAmountQueryResult>> Handle(GetThirtyLastOrderTotalAmountQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetOrderListLastThirtyOrderTotalAmount();
            return values.Select(x => new GetThirtyLastOrderTotalAmountQueryResult
            {
                TotalAmount = x.TotalAmount
            }).ToList();
        }
    }
}
