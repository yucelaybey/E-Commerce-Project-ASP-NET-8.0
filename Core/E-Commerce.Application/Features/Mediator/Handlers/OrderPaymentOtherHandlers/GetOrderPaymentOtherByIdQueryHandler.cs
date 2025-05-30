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
    public class GetOrderPaymentOtherByIdQueryHandler : IRequestHandler<GetOrderPaymentOtherByIdQuery, GetOrderPaymentOtherByIdQueryResult>
    {
        private readonly IRepository<OrderPaymentOther> _repository;

        public GetOrderPaymentOtherByIdQueryHandler(IRepository<OrderPaymentOther> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderPaymentOtherByIdQueryResult> Handle(GetOrderPaymentOtherByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetOrderPaymentOtherByIdQueryResult
            {
                OrderPaymentOtherId = values.OrderPaymentOtherId,
                PaymentName = values.PaymentName,
                PaymentNumber = values.PaymentNumber
            };
        }
    }
}
