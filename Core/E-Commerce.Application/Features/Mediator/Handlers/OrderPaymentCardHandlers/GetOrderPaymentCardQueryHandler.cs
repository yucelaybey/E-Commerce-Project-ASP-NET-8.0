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
    public class GetOrderPaymentCardQueryHandler : IRequestHandler<GetOrderPaymentCardQuery, List<GetOrderPaymentCardQueryResult>>
    {
        private readonly IRepository<OrderPaymentCard> _repository;

        public GetOrderPaymentCardQueryHandler(IRepository<OrderPaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderPaymentCardQueryResult>> Handle(GetOrderPaymentCardQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetOrderPaymentCardQueryResult
            {
                OrderPaymentCardId = x.OrderPaymentCardId,
                OrderCardName = x.OrderCardName,
                OrderCardNumber = x.OrderCardNumber,
                OrderCardDate = x.OrderCardDate,
                OrderCardCVV = x.OrderCardCVV
            }).ToList();
        }
    }
}
