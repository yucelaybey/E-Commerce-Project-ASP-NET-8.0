using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ShoppingCardItemDtos
{
    public class UpdateCartItemStatusDto
    {
        public int ShoppingCartItemId { get; set; }
        public bool Status { get; set; }
    }
}
