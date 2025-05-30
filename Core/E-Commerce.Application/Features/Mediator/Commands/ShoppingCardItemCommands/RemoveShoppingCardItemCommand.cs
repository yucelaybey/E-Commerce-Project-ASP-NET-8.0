using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands
{
    public class RemoveShoppingCardItemCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveShoppingCardItemCommand(int id)
        {
            Id = id;
        }
    }
}
