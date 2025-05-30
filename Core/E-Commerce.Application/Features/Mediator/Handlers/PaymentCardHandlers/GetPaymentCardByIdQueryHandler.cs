using E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries;
using E_Commerce.Application.Features.Mediator.Results.PaymentCardResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.PaymentCardHandlers
{
    public class GetPaymentCardByIdQueryHandler : IRequestHandler<GetPaymentCardByIdQuery, GetPaymentCardByIdQueryResult>
    {
        private readonly IRepository<PaymentCard> _repository;

        public GetPaymentCardByIdQueryHandler(IRepository<PaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task<GetPaymentCardByIdQueryResult> Handle(GetPaymentCardByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetPaymentCardByIdQueryResult
            {
                PaymentCardID = values.PaymentCardID,
                UserId = values.UserId,
                CardHolderName = values.CardHolderName,
                CardNumber = values.CardNumber,
                CVV = values.CVV,
                ExpirationDate = values.ExpirationDate,
                IsDefault = values.IsDefault
            };
        }
    }
}
