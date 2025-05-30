using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ProductQueries
{
    public class GetProductWithCategoryQuery : IRequest<GetProductWithCategoryQueryResult>
    {
        public int Id { get; set; }

        public GetProductWithCategoryQuery(int id)
        {
            Id = id;
        }
    }
}