using ECommerce.Dto.ProductShopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ShoppingCardItemDtos
{
    public class ShoppingCartWithTotalsDto
    {
        public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
        public decimal TotalPrice { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalDiscountRate { get; set; }
        public bool TotalStatus { get; set; }
        public int TrueStatusCount { get; set; }
        public List<ProductDto> RecommendedProducts { get; set; } = new List<ProductDto>();
    }
}
