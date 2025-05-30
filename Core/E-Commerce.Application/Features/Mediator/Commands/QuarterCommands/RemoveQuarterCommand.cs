using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.QuarterCommands
{
    public class RemoveQuarterCommand:IRequest
    {
        public int Id { get; set; }

        public RemoveQuarterCommand(int id)
        {
            Id = id;
        }
    }
}
