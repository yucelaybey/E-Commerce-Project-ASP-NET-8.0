using E_Commerce.Application.Features.Mediator.Results.PaymentCardResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries
{
    public class GetPaymentCardByIdQuery : IRequest<GetPaymentCardByIdQueryResult>
    {
        public int Id { get; set; }

        public GetPaymentCardByIdQuery(int id)
        {
            Id = id;
        }
    }
}
