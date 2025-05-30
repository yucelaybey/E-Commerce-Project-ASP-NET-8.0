using E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries
{
    public class GetShoppingCardQuery : IRequest<List<GetShoppingCardQueryResult>>
    {
    }
}
