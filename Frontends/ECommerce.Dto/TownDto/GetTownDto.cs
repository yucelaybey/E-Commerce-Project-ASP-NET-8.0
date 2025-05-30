using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.TownDto
{
    public class GetTownDto
    {
        public int TownId { get; set; }
        public int CityId { get; set; }
        public string TownName { get; set; }
    }
}
