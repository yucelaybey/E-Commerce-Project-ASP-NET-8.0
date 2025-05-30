using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands
{
    public class SetDefaultPaymentCardCommand : IRequest<bool>
    {
        public int PaymentCardID { get; set; }
        public int UserId { get; set; }
    }
}
