using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Dto.FavoritesCardItemDtos
{
    public class FavoritesCardItemDto
    {
        public int FavoritesCardItemID { get; set; }
        public int FavoritesCardID { get; set; }
        public int ProductID { get; set; }
    }
}
