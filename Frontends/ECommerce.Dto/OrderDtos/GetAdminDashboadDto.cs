using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderDtos
{
    public class GetAdminDashboadDto
    {
        public decimal TotalAmount { get; set; }
        public int TotalOrder { get; set; }
        public int TotalUser { get; set; }
        public int TotalProduct { get; set; }
    }
}
