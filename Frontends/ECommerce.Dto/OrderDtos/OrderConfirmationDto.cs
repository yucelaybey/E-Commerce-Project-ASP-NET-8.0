using E_Commerce.Domain.Entities;
using ECommerce.Dto.OrderItemDtos;
using ECommerce.Dto.OrderPaymentAddressDto;
using ECommerce.Dto.PaymentCardDtos;
using ECommerce.Dto.ShoppingCardItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class OrderConfirmationDto
    {
        public GetOrderDto Order { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
        public List<GetOrderItemDto> GetOrderItemDto { get; set; }
        public string UserEmail { get; set; }
        public GetOrderPaymentAddressDto AddressInfo { get; set; }
        public OrderTotalsDto Totals { get; set; }
    }
}
