using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
{
    public class RemoveShoppingCardCommandHandler : IRequestHandler<RemoveShoppingCardCommand>
    {
        private readonly IRepository<ShoppingCard> _repository;

        public RemoveShoppingCardCommandHandler(IRepository<ShoppingCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveShoppingCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
