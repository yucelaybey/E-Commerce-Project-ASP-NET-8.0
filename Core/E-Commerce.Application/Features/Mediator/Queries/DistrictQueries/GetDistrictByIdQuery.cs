using E_Commerce.Application.Features.Mediator.Results.DistrictResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.DistrictQueries
{
    public class GetDistrictByIdQuery:IRequest<GetDistrictByIdQueryResult>
    {
        public int Id { get; set; }

        public GetDistrictByIdQuery(int id)
        {
            Id = id;
        }
    }
}
