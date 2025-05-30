using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Dto.FavoritesCardDtos
{
    public class FavoritesCardDto
    {
        [JsonPropertyName("favoritesCardId")]
        public int favoritesCardId { get; set; }
    }
}
