using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class RemoveShoppingCardItemCommandHandler : IRequestHandler<RemoveShoppingCardItemCommand>
    {
        private readonly IRepository<ShoppingCardItem> _repository;

        public RemoveShoppingCardItemCommandHandler(IRepository<ShoppingCardItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveShoppingCardItemCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
