using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Dto.ProductShopping
{
    public class ProductDto
    {
        [JsonPropertyName("productID")]
        public int ProductID { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("salePrice")]
        public decimal SalePrice { get; set; }

        [JsonPropertyName("saleSeason")]
        public bool SaleSeason { get; set; }
    }
}
