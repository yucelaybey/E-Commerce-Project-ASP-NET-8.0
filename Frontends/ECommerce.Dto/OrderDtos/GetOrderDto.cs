using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class GetOrderDto
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; } // Siparişi veren kullanıcı
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public int? OrderPaymentCardId { get; set; }
        public int? OrderPaymentOtherId { get; set; }
        public int OrderPaymentAddressId { get; set; }
        public int TotalAmount { get; set; } // Toplam tutar


        public decimal TotalPrice { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal DiscountAmount { get; set; }

        public string NameSurname { get; set; }
    }
}
