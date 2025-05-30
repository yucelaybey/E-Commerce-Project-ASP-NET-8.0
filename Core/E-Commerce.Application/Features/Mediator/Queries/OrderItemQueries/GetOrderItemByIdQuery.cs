using E_Commerce.Application.Features.Mediator.Results.OrderItemResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries
{
    public class GetOrderItemByIdQuery : IRequest<GetOrderItemByIdQueryResult>
    {
        public int Id { get; set; }

        public GetOrderItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}
