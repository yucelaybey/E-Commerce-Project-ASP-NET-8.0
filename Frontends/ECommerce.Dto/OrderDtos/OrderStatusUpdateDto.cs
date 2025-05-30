using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class OrderStatusUpdateDto
    {
        public int orderId { get; set; }
        public string newStatus { get; set; }
    }
}
