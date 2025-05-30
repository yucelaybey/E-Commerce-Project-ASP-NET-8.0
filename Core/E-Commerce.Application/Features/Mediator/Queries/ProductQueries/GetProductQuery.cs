using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ProductQueries
{
    public class GetProductQuery : IRequest<List<GetProductQueryResult>>
    {
    }
}
