using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.AppUserCommands
{
    public class UpdateAppUserPasswordResetCommand:IRequest
    {
        public int AppUserId { get; set; }
        public string Password { get; set; }
    }
}
