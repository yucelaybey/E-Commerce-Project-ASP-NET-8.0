using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.TownResults
{
    public class GetTownQueryResult
    {
        public int TownId { get; set; }
        public int CityId { get; set; }
        public string TownName { get; set; }
    }
}
