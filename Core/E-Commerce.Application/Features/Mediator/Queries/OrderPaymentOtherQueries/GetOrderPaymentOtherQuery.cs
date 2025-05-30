using E_Commerce.Application.Features.Mediator.Results.OrderPaymentOrderResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderPaymentOtherQueries
{
    public class GetOrderPaymentOtherQuery:IRequest<List<GetOrderPaymentOtherQueryResult>>
    {
    }
}
