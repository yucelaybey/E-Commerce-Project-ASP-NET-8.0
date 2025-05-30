using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using E_Commerce.Application.Features.Mediator.Results.CityResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.CityQueries
{
    public class GetCityByIdQuery : IRequest<GetCityByIdQueryResult>
    {
        public int Id { get; set; }

        public GetCityByIdQuery(int id)
        {
            Id = id;
        }
    }
}
