using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderCommands
{
    public class UpdateOrderStatusCommand : IRequest<OrderStatusUpdateQueryResult>
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
    }
}
