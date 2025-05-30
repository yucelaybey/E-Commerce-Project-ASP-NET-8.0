using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using E_Commerce.Application.Features.Mediator.Results.OrderPaymentCardResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderPaymentCardQueries
{
    public class GetOrderPaymentCardByIdQuery:IRequest<GetOrderPaymentCardByIdQueryResult>
    {
        public int Id { get; set; }

        public GetOrderPaymentCardByIdQuery(int id)
        {
            Id = id;
        }
    }
}
