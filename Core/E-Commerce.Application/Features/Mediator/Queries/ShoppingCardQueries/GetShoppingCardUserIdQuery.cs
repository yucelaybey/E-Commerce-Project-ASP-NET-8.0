// E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries
using E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults;
using MediatR;

public class GetShoppingCardUserIdQuery : IRequest<ShoppingCardCheckResult>
{
    public string UserId { get; set; }

    public GetShoppingCardUserIdQuery(string userId)
    {
        UserId = userId;
    }
}