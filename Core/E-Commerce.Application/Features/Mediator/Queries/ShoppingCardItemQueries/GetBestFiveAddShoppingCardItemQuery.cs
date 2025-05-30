using ECommerce.Dto.ShoppingCardItem;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries
{
    public class GetBestFiveAddShoppingCardItemQuery : IRequest<List<BestFiveAddShoppingCardItemDto>>
    {
    }
}
