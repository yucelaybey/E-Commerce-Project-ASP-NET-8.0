using E_Commerce.Application.Features.Mediator.Commands.OrderCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IRepository<Order> _repository;

        public UpdateOrderCommandHandler(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderID);
            values.OrderStatus = request.OrderStatus;
            await _repository.UpdateAsync(values);
        }
    }
}
