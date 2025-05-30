using E_Commerce.Application.Features.Mediator.Commands.OrderItemCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand>
    {
        private readonly IRepository<OrderItem> _repository;

        public UpdateOrderItemCommandHandler(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderID);
            values.OrderStatus = request.OrderStatus;
            await _repository.UpdateAsync(values);
        }
    }
}
