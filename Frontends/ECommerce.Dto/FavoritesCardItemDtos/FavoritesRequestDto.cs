using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.FavoritesCardItemDtos
{
    public class FavoritesRequestDto
    {
        public string favoritesCardItemID { get; set; }
        public string favoritesCardID { get; set; }
        public int productID { get; set; }
        public string favorites { get; set; }
    }
}
