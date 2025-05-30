// E_Commerce.Application.Features.ShoppingCartItems.Queries
using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries;
using E_Commerce.Application.Interfaces.IShoppingCardItemCheckRepository;
using MediatR;

namespace E_Commerce.Application.Features.ShoppingCartItems.Queries
{
    public class CheckShoppingCardItemExistsQueryHandler : IRequestHandler<CheckShoppingCartItemExistsQuery, bool>
    {
        private readonly IShoppingCardItemCheckRepository _shoppingCardItemRepository;

        public CheckShoppingCardItemExistsQueryHandler(IShoppingCardItemCheckRepository shoppingCardItemRepository)
        {
            _shoppingCardItemRepository = shoppingCardItemRepository;
        }

        public async Task<bool> Handle(CheckShoppingCartItemExistsQuery request, CancellationToken cancellationToken)
        {
            return await _shoppingCardItemRepository.ExistsAsync(request.ProductId, request.ShoppingCardId);
        }
    }
}