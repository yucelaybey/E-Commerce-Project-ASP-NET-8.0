using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ShoppingCardItemDtos
{
    public class CreateShoppingCardItemDto
    {
        public int ProductID { get; set; }
        public int ShoppingCardID { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}
