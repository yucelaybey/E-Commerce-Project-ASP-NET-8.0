using E_Commerce.Application.Features.Mediator.Results.AppUserResults;
using E_Commerce.Application.Features.Mediator.Results.FavoritesCardResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries
{
    public class GetByIdProfilesQuery : IRequest<GetByIdProfilesQueryResult>
    {
        public GetByIdProfilesQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
