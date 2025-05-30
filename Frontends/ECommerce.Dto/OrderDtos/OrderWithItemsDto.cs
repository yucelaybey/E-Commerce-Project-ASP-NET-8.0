using ECommerce.Dto.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class OrderWithItemsDto
    {
        public GetOrderDto Order { get; set; }
        public List<GetOrderItemDto> OrderItems { get; set; }
    }
}
