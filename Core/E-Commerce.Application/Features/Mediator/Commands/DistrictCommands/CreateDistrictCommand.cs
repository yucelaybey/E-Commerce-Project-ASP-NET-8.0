using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.DistrictCommands
{
    public class CreateDistrictCommand:IRequest
    {
        public int CityId { get; set; }
        public int TownId { get; set; }
        public int QuarterId { get; set; }
        public string DistrictName { get; set; }
        public int PostalCode { get; set; }
    }
}
