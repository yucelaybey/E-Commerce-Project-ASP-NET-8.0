using E_Commerce.Application.Features.Mediator.Results.QuarterResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.QuarterQueries
{
    public class GetQuarterByIdQuery:IRequest<GetQuarterByIdQueryResult>
    {
        public int Id { get; set; }

        public GetQuarterByIdQuery(int id)
        {
            Id = id;
        }
    }
}
