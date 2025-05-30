using E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands;
using E_Commerce.Application.Interfaces.IPaymentCardRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class SetDefaultPaymentCardCommandHandler : IRequestHandler<SetDefaultPaymentCardCommand, bool>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;

        public SetDefaultPaymentCardCommandHandler(IPaymentCardRepository paymentCardRepository)
        {
            _paymentCardRepository = paymentCardRepository;
        }

        public async Task<bool> Handle(SetDefaultPaymentCardCommand request, CancellationToken cancellationToken)
        {
            // Get all cards for the user
            var userCards = await _paymentCardRepository.GetPaymentCardsByUserIdAsync(request.UserId);

            // Set all cards to IsDefault = false
            foreach (var card in userCards)
            {
                if (card.IsDefault)
                {
                    card.IsDefault = false;
                    await _paymentCardRepository.UpdatePaymentCardAsync(card);
                }
            }

            // Find and update the specified card
            var cardToSetDefault = await _paymentCardRepository.GetPaymentCardByIdAsync(request.PaymentCardID);

            if (cardToSetDefault == null)
            {
                return false;
            }

            if (cardToSetDefault.UserId != request.UserId)
            {
                return false;
            }

            cardToSetDefault.IsDefault = true;
            await _paymentCardRepository.UpdatePaymentCardAsync(cardToSetDefault);

            return true;
        }
    }
}
