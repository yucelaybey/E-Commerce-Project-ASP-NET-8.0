using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using ECommerce.Dto.OrderItemDtos;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries
{
    public class GetOrderItemBestSellProductQuery : IRequest<List<BestSellerProductDto>>
    {
        public GetOrderItemBestSellProductQuery()
        {
        }
    }
}
