using E_Commerce.Application.Features.Mediator.Results.OrderItemResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries
{
    public class GetOrderItemQuery : IRequest<List<GetOrderItemQueryResult>>
    {
    }
}
