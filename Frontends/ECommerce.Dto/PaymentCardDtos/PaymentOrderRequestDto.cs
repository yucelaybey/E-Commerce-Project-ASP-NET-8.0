using ECommerce.Dto.ShoppingCardItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.PaymentCardDtos
{
    public class PaymentOrderRequestDto
    {
        public string PaymentMethod { get; set; }
        public PaymentDataDto PaymentData { get; set; }
        public ShoppingCartWithTotalsDto CartData { get; set; }
        public int SelectedAddressId { get; set; } // Yeni eklenen alan
    }
}
