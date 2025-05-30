using E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries
{
    public class GetShoppingCardByIdQuery : IRequest<GetShoppingCardByIdQueryResult>
    {
        public int Id { get; set; }

        public GetShoppingCardByIdQuery(int id)
        {
            Id = id;
        }
    }
}
