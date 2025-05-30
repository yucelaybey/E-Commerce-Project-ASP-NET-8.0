using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries;
using E_Commerce.Application.Features.Mediator.Results.ShoppingCardItemResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class GetShoppingCardItemByIdQueryHandler : IRequestHandler<GetShoppingCardItemByIdQuery, GetShoppingCardItemByIdQueryResult>
    {
        private readonly IRepository<ShoppingCardItem> _repository;

        public GetShoppingCardItemByIdQueryHandler(IRepository<ShoppingCardItem> repository)
        {
            _repository = repository;
        }

        public async Task<GetShoppingCardItemByIdQueryResult> Handle(GetShoppingCardItemByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetShoppingCardItemByIdQueryResult
            {
                ShoppingCardItemID = values.ShoppingCardItemID,
                ProductID = values.ProductID,
                Quantity = values.Quantity,
                TotalPrice = values.TotalPrice,
                Status = values.Status
            };
        }
    }
}
