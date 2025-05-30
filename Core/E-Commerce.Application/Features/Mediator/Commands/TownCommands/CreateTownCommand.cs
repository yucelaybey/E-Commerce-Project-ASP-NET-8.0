using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.TownCommands
{
    public class CreateTownCommand:IRequest
    {
        public int CityId { get; set; }
        public string TownName { get; set; }
    }
}
