using E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries;
using E_Commerce.Application.Interfaces.IPaymentCardRepository;
using ECommerce.Dto.PaymentCardDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class GetUserPaymentCardsHandler : IRequestHandler<GetUserPaymentCardsResult, List<GetPaymentCardDto>>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;

        public GetUserPaymentCardsHandler(IPaymentCardRepository paymentCardRepository)
        {
            _paymentCardRepository = paymentCardRepository;
        }

        public async Task<List<GetPaymentCardDto>> Handle(GetUserPaymentCardsResult request, CancellationToken cancellationToken)
        {
            var paymentCards = await _paymentCardRepository.GetPaymentCardsByUserIdAsync(request.UserId);

            return paymentCards.Select(pc => new GetPaymentCardDto
            {
                UserId = request.UserId,
                CardHolderName = pc.CardHolderName,
                CardNumber = pc.CardNumber,
                CVV = pc.CVV,
                ExpirationDate = pc.ExpirationDate,
                IsDefault = pc.IsDefault,
                PaymentCardId = pc.PaymentCardID
            }).ToList();
        }
    }
}
