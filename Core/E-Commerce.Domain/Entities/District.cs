using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class District
    {
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int TownId { get; set; }
        public Town Town { get; set; }
        public int QuarterId { get; set; }
        public Quarter Quarter { get; set; }
        public string DistrictName { get; set; }
        public int PostalCode { get; set; }
    }
}
