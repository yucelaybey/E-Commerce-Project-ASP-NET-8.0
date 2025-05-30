using ECommerce.Dto.AddressDto;
using ECommerce.Dto.OrderPaymentAddressDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class OrderCustomerDto
    {
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public GetOrderPaymentAddressDto GetOrderPaymentAddressDto { get; set; }
    }
}
