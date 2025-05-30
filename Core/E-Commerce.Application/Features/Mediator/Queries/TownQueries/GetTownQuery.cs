using E_Commerce.Application.Features.Mediator.Results.TownResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.TownQueries
{
    public class GetTownQuery:IRequest<List<GetTownQueryResult>>
    {
    }
}
