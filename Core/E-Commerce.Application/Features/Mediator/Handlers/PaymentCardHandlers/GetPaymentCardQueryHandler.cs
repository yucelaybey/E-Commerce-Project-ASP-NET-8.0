using E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries;
using E_Commerce.Application.Features.Mediator.Results.PaymentCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class GetPaymentCardQueryHandler : IRequestHandler<GetPaymentCardQuery, List<GetPaymentCardQueryResult>>
    {
        private readonly IRepository<PaymentCard> _repository;

        public GetPaymentCardQueryHandler(IRepository<PaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetPaymentCardQueryResult>> Handle(GetPaymentCardQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetPaymentCardQueryResult
            {
                PaymentCardID = x.PaymentCardID,
                UserId = x.UserId,
                CardHolderName = x.CardHolderName,
                CardNumber = x.CardNumber,
                CVV = x.CVV,
                ExpirationDate = x.ExpirationDate,
                IsDefault = x.IsDefault
            }).ToList();
        }
    }
}
