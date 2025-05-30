using ECommerce.Dto.PaymentCardDtos;
using MediatR;
namespace E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands
{
    public class CreatePaymentCardCommand : IRequest<CreatedPaymentCardResponseDto>
    {
        public CreatePaymentCardDto CreatePaymentCardDto { get; set; }
    }
}
