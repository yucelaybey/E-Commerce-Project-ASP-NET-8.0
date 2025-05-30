using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Town
    {
        public int TownId { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
        public string TownName { get; set; }
    }
}
