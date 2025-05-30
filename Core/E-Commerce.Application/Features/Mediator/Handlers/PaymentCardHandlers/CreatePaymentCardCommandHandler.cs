using E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.IPaymentCardRepository;
using E_Commerce.Domain.Entities;
using ECommerce.Dto.PaymentCardDtos;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class CreatePaymentCardCommandHandler : IRequestHandler<CreatePaymentCardCommand, CreatedPaymentCardResponseDto>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;

        public CreatePaymentCardCommandHandler(IPaymentCardRepository paymentCardRepository)
        {
            _paymentCardRepository = paymentCardRepository;
        }

        public async Task<CreatedPaymentCardResponseDto> Handle(CreatePaymentCardCommand request, CancellationToken cancellationToken)
        {
            var paymentCard = new PaymentCard
            {
                UserId = request.CreatePaymentCardDto.UserId,
                CardNumber = request.CreatePaymentCardDto.CardNumber,
                CardHolderName = request.CreatePaymentCardDto.CardHolderName,
                ExpirationDate = DateTime.Parse(request.CreatePaymentCardDto.ExpirationDate),
                CVV = request.CreatePaymentCardDto.CVV,
                IsDefault = request.CreatePaymentCardDto.IsDefault
            };

            var createdCard = await _paymentCardRepository.AddAsync(paymentCard);

            return new CreatedPaymentCardResponseDto
            {
                PaymentCardId = createdCard.PaymentCardID,
                Success = true,
                Message = "Kart başarıyla oluşturuldu"
            };
        }
    }
}
