using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class FavoritesCardItem
    {
        public int FavoritesCardItemID { get; set; }
        public int FavoritesCardID { get; set; }
        public FavoritesCard FavoritesCard { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
