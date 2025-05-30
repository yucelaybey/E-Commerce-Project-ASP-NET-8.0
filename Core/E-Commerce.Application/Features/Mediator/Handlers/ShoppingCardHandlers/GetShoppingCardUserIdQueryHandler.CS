// E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries;
using E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults;
using E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetShoppingCardUserIdQueryHandler : IRequestHandler<GetShoppingCardUserIdQuery, ShoppingCardCheckResult>
{
    private readonly IShoppingCardGetUserIdRepository _repository;

    public GetShoppingCardUserIdQueryHandler(IShoppingCardGetUserIdRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShoppingCardCheckResult> Handle(GetShoppingCardUserIdQuery request, CancellationToken cancellationToken)
    {
        var (exists, shoppingCardId) = await _repository.GetShoppingCardUserId(request.UserId);
        return new ShoppingCardCheckResult
        {
            HasShoppingCard = exists,
            ShoppingCardId = shoppingCardId
        };
    }
}