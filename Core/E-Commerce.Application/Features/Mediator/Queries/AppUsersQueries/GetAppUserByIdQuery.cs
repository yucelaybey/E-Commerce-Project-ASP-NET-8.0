using E_Commerce.Application.Features.Mediator.Results.AppUserResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries
{
    public class GetAppUserByIdQuery:IRequest<GetAppUserByIdQueryResult>
    {
        public GetAppUserByIdQuery(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
