using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.FavoritesCardItemDtos
{
    public class FavoriteCheckResultDto
    {
        public int? FavoritesCardID { get; set; }
        public int? ProductID { get; set; }
        public int? FavoritesCardItemID { get; set; }
        public bool Status { get; set; }
    }
}
