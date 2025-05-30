using ECommerce.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.FavoritesCardItemDtos
{
    public class FavoritesCardItemWithProductDto
    {
        public int FavoritesCardItemId { get; set; }
        public int FavoritesCardId { get; set; }
        public GetProductDto Product { get; set; }
    }
}
