using E_Commerce.Domain.Entities;
using ECommerce.Dto.ShoppingCardItemDtos;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands
{
    public class CreateShoppingCardItemCommand : IRequest
    {
        public CreateShoppingCardItemDto ItemDto { get; set; }
    }
}
