using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderPaymentAddressCommands
{
    public class CreateOrderPaymentAddressCommand:IRequest<int>
    {
        public string AddressTitle { get; set; }
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Quarter { get; set; }
        public string District { get; set; }
        public string AddressDetails { get; set; }
    }
}
