using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ProductCommands
{
    public class RemoveProductCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveProductCommand(int id)
        {
            Id = id;
        }
    }
}
