using E_Commerce.Application.Features.Mediator.Results.OrderPaymentCardResults;
using E_Commerce.Application.Features.Mediator.Results.QuarterResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderPaymentCardQueries
{
    public class GetOrderPaymentCardQuery : IRequest<List<GetOrderPaymentCardQueryResult>>
    {
    }
}
