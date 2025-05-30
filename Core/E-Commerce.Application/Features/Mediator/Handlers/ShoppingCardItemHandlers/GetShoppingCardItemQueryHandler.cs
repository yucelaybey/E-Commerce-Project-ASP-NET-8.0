using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries;
using E_Commerce.Application.Features.Mediator.Results.ShoppingCardItemResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class GetShoppingCardItemQueryHandler : IRequestHandler<GetShoppingCardItemQuery, List<GetShoppingCardItemQueryResult>>
    {
        private readonly IRepository<ShoppingCardItem> _repository;

        public GetShoppingCardItemQueryHandler(IRepository<ShoppingCardItem> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetShoppingCardItemQueryResult>> Handle(GetShoppingCardItemQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetShoppingCardItemQueryResult
            {
                ShoppingCardItemID = x.ShoppingCardItemID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                ShoppingCardID =x.ShoppingCardID,
                Status = x.Status,
                TotalPrice = x.TotalPrice
            }).ToList();
        }
    }
}
