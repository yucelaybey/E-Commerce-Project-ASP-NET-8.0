using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries;
using E_Commerce.Application.Interfaces.IBestFiveShoppingCardAddProductRepository;
using ECommerce.Dto.ShoppingCardItem;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class GetBestFiveAddShoppingCardItemQueryHandler : IRequestHandler<GetBestFiveAddShoppingCardItemQuery, List<BestFiveAddShoppingCardItemDto>>
    {
        private readonly IBestFiveShoppingCardAddProductRepository _repository;

        public GetBestFiveAddShoppingCardItemQueryHandler(IBestFiveShoppingCardAddProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BestFiveAddShoppingCardItemDto>> Handle(GetBestFiveAddShoppingCardItemQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMostAddedProductsAsync();
        }
    }
}
