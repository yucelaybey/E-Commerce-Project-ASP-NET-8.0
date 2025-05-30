using E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class RemovePaymentCardCommandHandler : IRequestHandler<RemovePaymentCardCommand>
    {
        private readonly IRepository<PaymentCard> _repository;

        public RemovePaymentCardCommandHandler(IRepository<PaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemovePaymentCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
