using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands
{
    public class RemovePaymentCardCommand : IRequest
    {
        public int Id { get; set; }

        public RemovePaymentCardCommand(int id)
        {
            Id = id;
        }
    }
}
