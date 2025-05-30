using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ProductDtos
{
    public class UpdateProductStockResponseDto
    {
        public int ProductId { get; set; }
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
