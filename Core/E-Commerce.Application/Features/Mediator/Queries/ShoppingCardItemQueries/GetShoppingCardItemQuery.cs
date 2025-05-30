using E_Commerce.Application.Features.Mediator.Results.ShoppingCardItemResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries
{
    public class GetShoppingCardItemQuery : IRequest<List<GetShoppingCardItemQueryResult>>
    {
    }
}
