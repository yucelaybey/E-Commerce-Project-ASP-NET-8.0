using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands
{
    public class UpdatePaymentCardCommand : IRequest
    {
        public int PaymentCardID { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
        public bool IsDefault { get; set; } // Varsayılan kart
    }
}
