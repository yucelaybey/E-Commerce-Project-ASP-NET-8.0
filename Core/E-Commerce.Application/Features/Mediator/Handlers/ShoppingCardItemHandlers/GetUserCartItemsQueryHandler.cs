using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries;
using E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories;
using ECommerce.Dto.ShoppingCardItemDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardItemHandlers
{
    public class GetUserCartItemsQueryHandler : IRequestHandler<GetUserCartItemsQuery, ShoppingCartWithTotalsDto>
    {
        private readonly IShoppingCardGetUserIdRepository _repository;

        public GetUserCartItemsQueryHandler(IShoppingCardGetUserIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShoppingCartWithTotalsDto> Handle(GetUserCartItemsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetUserCartItemsWithTotalsAsync(request.UserId);
        }
    }
}
