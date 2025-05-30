using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ShoppingCardItemDtos
{
    public class ShoppingCartItemDto
    {
        public int ProductId { get; set; }
        public int ShoppingCartItemID { get; set; }
        public int ShoppingCartID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal ProductPrice { get; set; }
        public double ProductDiscountRate { get; set; }
        public bool ProductSaleSeason { get; set; }
        public int Quantity { get; set; }
        public int ProductSalePrice { get; set; }
        public int ProductStock { get; set; }
        public bool Status { get; set; }

        public int? FavoritesCardItemID { get; set; }
        public int? FavoritesCardID { get; set; }
        public bool? Favorites { get; set; }

        public decimal ItemTotalPrice =>  Status ? ProductPrice * Quantity : 0;
        public decimal ItemTotalSalePrice => Status ? ProductSalePrice * Quantity : 0;
        public decimal ItemDiscountAmount => Status ? ItemTotalPrice - ItemTotalSalePrice : 0;
        public decimal ItemDiscountRate => ProductPrice > 0 ?
            Math.Round(((ProductPrice - ProductSalePrice) / ProductPrice) * 100, 2) : 0;
    }
}
