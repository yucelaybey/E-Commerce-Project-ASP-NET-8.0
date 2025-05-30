using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
{
    public class UpdateShoppingCardQueryHandler : IRequestHandler<UpdateShoppingCardCommand>
    {
        private readonly IRepository<ShoppingCard> _repository;

        public UpdateShoppingCardQueryHandler(IRepository<ShoppingCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateShoppingCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.ShoppingCardID);
            values.UserId = request.UserId;
            await _repository.UpdateAsync(values);
        }
    }
}
