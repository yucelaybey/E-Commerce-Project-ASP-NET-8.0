using E_Commerce.Application.Features.Mediator.Results.CategoryResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.CategoryQueries
{
    public class GetCategoryQuery : IRequest<List<GetCategoryQueryResult>>
    {
    }
}
