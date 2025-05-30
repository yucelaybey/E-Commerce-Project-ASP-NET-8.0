using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries;
using E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
{
    public class GetShoppingCardQueryHandler : IRequestHandler<GetShoppingCardQuery, List<GetShoppingCardQueryResult>>
    {
        private readonly IRepository<ShoppingCard> _repository;

        public GetShoppingCardQueryHandler(IRepository<ShoppingCard> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetShoppingCardQueryResult>> Handle(GetShoppingCardQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetShoppingCardQueryResult
            {
                ShoppingCardID = x.ShoppingCardID,
                UserId = x.UserId
            }).ToList();
        }
    }
}
