using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands
{
    public class UpdateShoppingCardCommand : IRequest
    {
        public int ShoppingCardID { get; set; }
        public string UserId { get; set; } // Kullanıcı adı ile ilişkilendirme
    }
}
