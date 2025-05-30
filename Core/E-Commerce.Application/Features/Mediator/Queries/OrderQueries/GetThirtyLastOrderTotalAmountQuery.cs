using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderQueries
{
    public class GetThirtyLastOrderTotalAmountQuery : IRequest<List<GetThirtyLastOrderTotalAmountQueryResult>>
    {
    }
}
