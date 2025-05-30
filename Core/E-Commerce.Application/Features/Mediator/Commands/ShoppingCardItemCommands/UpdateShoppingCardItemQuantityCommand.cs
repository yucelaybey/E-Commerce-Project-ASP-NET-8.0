using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands
{
    public class UpdateShoppingCardItemQuantityCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public int ShoppingCardId { get; set; }
        public int QuantityToAdd { get; set; }
    }
}
