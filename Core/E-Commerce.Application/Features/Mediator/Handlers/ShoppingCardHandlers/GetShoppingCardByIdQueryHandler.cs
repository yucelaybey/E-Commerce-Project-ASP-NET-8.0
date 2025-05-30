using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries;
using E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
{
    public class GetShoppingCardByIdQueryHandler : IRequestHandler<GetShoppingCardByIdQuery, GetShoppingCardByIdQueryResult>
    {
        private readonly IRepository<ShoppingCard> _repository;

        public GetShoppingCardByIdQueryHandler(IRepository<ShoppingCard> repository)
        {
            _repository = repository;
        }

        public async Task<GetShoppingCardByIdQueryResult> Handle(GetShoppingCardByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetShoppingCardByIdQueryResult
            {
                ShoppingCardID = values.ShoppingCardID,
                UserId = values.UserId
            };
        }
    }
}
