using E_Commerce.Application.Features.Mediator.Results.PaymentCardResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries
{
    public class GetPaymentCardQuery : IRequest<List<GetPaymentCardQueryResult>>
    {
    }
}
