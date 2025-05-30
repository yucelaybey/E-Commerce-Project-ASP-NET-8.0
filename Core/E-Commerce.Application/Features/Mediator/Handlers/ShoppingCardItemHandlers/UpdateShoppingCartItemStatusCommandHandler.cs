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
    public class UpdateShoppingCartItemStatusCommandHandler : IRequestHandler<UpdateShoppingCartItemStatusCommand, bool>
    {
        private readonly IShoppingCardGetUserIdRepository _repository;

        public UpdateShoppingCartItemStatusCommandHandler(IShoppingCardGetUserIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateShoppingCartItemStatusCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.ShoppingCartItemId);
            if (item == null) return false;

            item.Status = request.Status;
            await _repository.UpdateAsync(item);
            return true;
        }
    }
}
