using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
{
    public class CreateShoppingCardCommandHandler : IRequestHandler<CreateShoppingCardCommand>
    {
        private readonly IRepository<ShoppingCard> _repository;

        public CreateShoppingCardCommandHandler(IRepository<ShoppingCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateShoppingCardCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new ShoppingCard
            {
                UserId = request.UserId
            });
        }
    }
}
