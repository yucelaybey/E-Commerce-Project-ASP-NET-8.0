using E_Commerce.Application.Features.Mediator.Results.AppUserResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries
{
    public class GetCheckAppUserQuery:IRequest<GetCheckAppUserQueryResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
