using E_Commerce.Application.Features.Mediator.Commands.OrderItemCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand>
    {
        private readonly IRepository<OrderItem> _repository;

        public RemoveOrderItemCommandHandler(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
