using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.TownCommands
{
    public class RemoveTownCommand:IRequest
    {
        public int Id { get; set; }

        public RemoveTownCommand(int id)
        {
            Id = id;
        }
    }
}
