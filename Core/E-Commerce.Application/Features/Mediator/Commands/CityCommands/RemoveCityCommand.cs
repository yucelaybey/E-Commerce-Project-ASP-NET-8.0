using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.CityCommands
{
    public class RemoveCityCommand:IRequest
    {
        public int Id { get; set; }

        public RemoveCityCommand(int id)
        {
            Id = id;
        }
    }
}
