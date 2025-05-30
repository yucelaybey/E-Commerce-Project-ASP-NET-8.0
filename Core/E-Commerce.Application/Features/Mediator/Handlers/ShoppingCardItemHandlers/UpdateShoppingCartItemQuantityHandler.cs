using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands;
using E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class UpdateShoppingCartItemQuantityHandler : IRequestHandler<UpdateShoppingCartItemQuantityCommand>
    {
        private readonly IShoppingCardGetUserIdRepository _repository;

        public UpdateShoppingCartItemQuantityHandler(IShoppingCardGetUserIdRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateShoppingCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            await _repository.UpdateQuantityAsync(
                request.ShoppingCartItemId,
                request.Quantity);
        }
    }
}
