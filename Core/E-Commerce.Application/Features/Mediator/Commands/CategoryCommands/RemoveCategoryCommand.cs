using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.CategoryCommands
{
    public class RemoveCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
