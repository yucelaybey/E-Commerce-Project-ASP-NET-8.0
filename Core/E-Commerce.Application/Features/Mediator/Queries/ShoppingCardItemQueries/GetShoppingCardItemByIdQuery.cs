using E_Commerce.Application.Features.Mediator.Results.ShoppingCardItemResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries
{
    public class GetShoppingCardItemByIdQuery : IRequest<GetShoppingCardItemByIdQueryResult>
    {
        public int Id { get; set; }

        public GetShoppingCardItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}
