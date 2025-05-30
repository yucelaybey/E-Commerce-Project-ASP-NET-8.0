using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands
{
    public class RemoveShoppingCardCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveShoppingCardCommand(int id)
        {
            Id = id;
        }
    }
}
