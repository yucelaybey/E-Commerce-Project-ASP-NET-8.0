using E_Commerce.Application.Features.Mediator.Queries.OrderPaymentCardQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderPaymentCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentCardHandlers
{
    public class GetOrderPaymentCardByIdQueryHandler : IRequestHandler<GetOrderPaymentCardByIdQuery, GetOrderPaymentCardByIdQueryResult>
    {
        private readonly IRepository<OrderPaymentCard> _repository;

        public GetOrderPaymentCardByIdQueryHandler(IRepository<OrderPaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderPaymentCardByIdQueryResult> Handle(GetOrderPaymentCardByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetOrderPaymentCardByIdQueryResult
            {
                OrderPaymentCardId = values.OrderPaymentCardId,
                OrderCardName = values.OrderCardName,
                OrderCardNumber = values.OrderCardNumber,
                OrderCardDate = values.OrderCardDate,
                OrderCardCVV = values.OrderCardCVV
            };
        }
    }
}
