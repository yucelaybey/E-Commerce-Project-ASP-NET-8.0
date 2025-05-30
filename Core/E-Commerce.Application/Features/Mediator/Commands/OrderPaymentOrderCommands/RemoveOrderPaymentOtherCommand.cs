using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderPaymentOrderCommands
{
    public class RemoveOrderPaymentOtherCommand:IRequest
    {
        public int Id { get; set; }

        public RemoveOrderPaymentOtherCommand(int id)
        {
            Id = id;
        }
    }
}
