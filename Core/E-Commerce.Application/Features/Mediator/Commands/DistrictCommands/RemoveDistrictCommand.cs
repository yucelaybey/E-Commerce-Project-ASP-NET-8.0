using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace E_Commerce.Application.Features.Mediator.Commands.DistrictCommands
{
    public class RemoveDistrictCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveDistrictCommand(int id)
        {
            Id = id;
        }
    }
}
