using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.IShoppingCardItemCheckRepository;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class CreateShoppingCardItemCommandHandler : IRequestHandler<CreateShoppingCardItemCommand>
    {
        private readonly IShoppingCardItemCheckRepository _repository;

        public CreateShoppingCardItemCommandHandler(IShoppingCardItemCheckRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateShoppingCardItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new ShoppingCardItem
            {
                ProductID = request.ItemDto.ProductID,
                ShoppingCardID = request.ItemDto.ShoppingCardID,
                Quantity = request.ItemDto.Quantity,
                TotalPrice = request.ItemDto.TotalPrice,
                Status = request.ItemDto.Status
            });
        }
    }
}
