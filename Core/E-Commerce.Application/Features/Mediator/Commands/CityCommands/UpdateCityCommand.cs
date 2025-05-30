using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.CityCommands
{
    public class UpdateCityCommand : IRequest
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
