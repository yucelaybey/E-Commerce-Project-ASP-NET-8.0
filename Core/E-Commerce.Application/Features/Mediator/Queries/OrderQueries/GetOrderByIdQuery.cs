using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderQueries
{
    public class GetOrderByIdQuery : IRequest<GetOrderByIdQueryResult>
    {
        public int Id { get; set; }

        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }
}
