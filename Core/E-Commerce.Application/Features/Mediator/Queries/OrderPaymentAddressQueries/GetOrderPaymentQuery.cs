using E_Commerce.Application.Features.Mediator.Results.OrderItemResults;
using E_Commerce.Application.Features.Mediator.Results.OrderPaymentAddressResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderPaymentAddressQueries
{
    public class GetOrderPaymentQuery : IRequest<List<GetOrderPaymentAddressQueryResult>>
    {
    }
}
