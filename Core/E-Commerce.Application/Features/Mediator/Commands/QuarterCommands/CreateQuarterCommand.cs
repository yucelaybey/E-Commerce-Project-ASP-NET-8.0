using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.QuarterCommands
{
    public class CreateQuarterCommand : IRequest
    {
        public int CityId { get; set; }
        public int TownId { get; set; }
        public string QuarterName { get; set; }
    }
}
