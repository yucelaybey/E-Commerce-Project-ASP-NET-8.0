using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands;
using E_Commerce.Application.Interfaces.IShoppingCardItemCheckRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class UpdateShoppingCardItemQuantityCommandHandler : IRequestHandler<UpdateShoppingCardItemQuantityCommand, Unit>
    {
        private readonly IShoppingCardItemCheckRepository _shoppingCardItemRepository;

        public UpdateShoppingCardItemQuantityCommandHandler(IShoppingCardItemCheckRepository shoppingCardItemRepository)
        {
            _shoppingCardItemRepository = shoppingCardItemRepository;
        }

        public async Task<Unit> Handle(UpdateShoppingCardItemQuantityCommand request, CancellationToken cancellationToken)
        {
            await _shoppingCardItemRepository.UpdateQuantityAsync(
                request.ProductId,
                request.ShoppingCardId,
                request.QuantityToAdd);

            return Unit.Value;
        }
    }
}
