using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.QuarterDto
{
    public class GetQuarterDto
    {
        public int QuarterId { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        public string QuarterName { get; set; }
    }
}
