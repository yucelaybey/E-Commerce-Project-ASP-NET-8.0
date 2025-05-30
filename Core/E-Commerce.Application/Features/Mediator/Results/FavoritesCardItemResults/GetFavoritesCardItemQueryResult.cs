using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.FavoritesCardItemResults
{
    public class GetFavoritesCardItemQueryResult
    {
        public int FavoritesCardItemID { get; set; }
        public int FavoritesCardID { get; set; }
        public int ProductID { get; set; }
    }
}
