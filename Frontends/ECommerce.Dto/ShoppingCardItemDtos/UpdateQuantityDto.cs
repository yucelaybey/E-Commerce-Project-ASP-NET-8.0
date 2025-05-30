using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ShoppingCardItemDtos
{
    public class UpdateQuantityDto
    {
        public int ShoppingCardItemID { get; set; }
        public int Quantity { get; set; }
    }
}
