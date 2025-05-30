using E_Commerce.Application.Features.Mediator.Queries.OrderPaymentOtherQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderPaymentOrderResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentOtherHandlers
{
    public class GetOrderPaymentOtherQueryHandler : IRequestHandler<GetOrderPaymentOtherQuery, List<GetOrderPaymentOtherQueryResult>>
    {
        private readonly IRepository<OrderPaymentOther> _repository;

        public GetOrderPaymentOtherQueryHandler(IRepository<OrderPaymentOther> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderPaymentOtherQueryResult>> Handle(GetOrderPaymentOtherQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetOrderPaymentOtherQueryResult
            {
                OrderPaymentOtherId = x.OrderPaymentOtherId,
                PaymentName = x.PaymentName,
                PaymentNumber =x.PaymentNumber
            }).ToList();
        }
    }
}
