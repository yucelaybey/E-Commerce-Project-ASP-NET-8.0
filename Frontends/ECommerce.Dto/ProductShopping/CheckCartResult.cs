using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Dto.ProductShopping
{
    public class CheckCartResult
    {
        [JsonPropertyName("hasShoppingCard")] // JSON'daki isimle eşleşmeli
        public bool HasShoppingCard { get; set; }

        [JsonPropertyName("shoppingCardId")] // JSON'daki isimle eşleşmeli
        public int? ShoppingCardId { get; set; }
    }
}
