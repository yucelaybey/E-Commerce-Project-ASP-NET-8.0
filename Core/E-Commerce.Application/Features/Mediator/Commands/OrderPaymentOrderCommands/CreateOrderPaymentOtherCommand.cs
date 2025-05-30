using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderPaymentOrderCommands
{
    public class CreateOrderPaymentOtherCommand:IRequest<int>
    {
        public string PaymentName { get; set; }
        public string PaymentNumber { get; set; }
    }
}
