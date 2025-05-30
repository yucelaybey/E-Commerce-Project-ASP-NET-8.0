using ECommerce.Dto.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class OrderTotalsDto
    {
        public decimal TotalPrice { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public double TotalDiscountRate { get; set; }
        public bool TotalStatus { get; set; }
        public int TrueStatusCount { get; set; }
        public List<GetOrderItemDto> Items { get; set; }
    }
}
