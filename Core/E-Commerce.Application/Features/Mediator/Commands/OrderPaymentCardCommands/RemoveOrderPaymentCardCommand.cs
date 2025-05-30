using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderPaymentCardCommands
{
    public class RemoveOrderPaymentCardCommand:IRequest
    {
        public int Id { get; set; }

        public RemoveOrderPaymentCardCommand(int id)
        {
            Id = id;
        }
    }
}
