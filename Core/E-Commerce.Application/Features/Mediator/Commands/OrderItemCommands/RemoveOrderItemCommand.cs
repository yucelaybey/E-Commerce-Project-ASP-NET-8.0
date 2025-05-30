using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderItemCommands
{
    public class RemoveOrderItemCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveOrderItemCommand(int id)
        {
            Id = id;
        }
    }
}
