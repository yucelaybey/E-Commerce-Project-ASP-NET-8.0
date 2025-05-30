using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands
{
    public class UpdateShoppingCartItemQuantityCommand : IRequest
    {
        public int ShoppingCartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
