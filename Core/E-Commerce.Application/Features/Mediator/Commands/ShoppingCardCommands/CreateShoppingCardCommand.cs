using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands
{
    public class CreateShoppingCardCommand : IRequest
    {
        public string UserId { get; set; } // Kullanıcı adı ile ilişkilendirme
    }
}
