using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.CategoryCommands
{
    public class CreateCategoryCommand : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
