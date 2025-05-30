using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderCommands
{
    public class RemoveOrderCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveOrderCommand(int id)
        {
            Id = id;
        }
    }
}
