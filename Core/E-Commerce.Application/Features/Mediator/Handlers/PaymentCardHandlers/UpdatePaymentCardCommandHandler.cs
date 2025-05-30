using E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class UpdatePaymentCardCommandHandler : IRequestHandler<UpdatePaymentCardCommand>
    {
        private readonly IRepository<PaymentCard> _repository;

        public UpdatePaymentCardCommandHandler(IRepository<PaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdatePaymentCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.PaymentCardID);
            values.CardNumber = request.CardNumber;
            values.CardHolderName = request.CardHolderName;
            values.CVV = request.CVV;
            values.ExpirationDate = request.ExpirationDate;
            values.IsDefault = request.IsDefault;
            await _repository.UpdateAsync(values);
        }
    }
}
