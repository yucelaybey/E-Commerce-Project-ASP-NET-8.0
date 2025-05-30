using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class UpdateShoppingCardItemCommandHandler : IRequestHandler<UpdateShoppingCardItemCommand>
    {
        private readonly IRepository<ShoppingCardItem> _repository;

        public UpdateShoppingCardItemCommandHandler(IRepository<ShoppingCardItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateShoppingCardItemCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.ShoppingCardItemID);
            values.Quantity = request.Quantity;
            values.TotalPrice = request.TotalPrice;
            values.Status = request.Status;
            await _repository.UpdateAsync(values);
        }
    }
}
