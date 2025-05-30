using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries;
using E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.ShoppingCardHandlers
{
    public class GetTotalQuantityByUserIdQueryHandler : IRequestHandler<GetTotalQuantityByUserIdQuery, int?>
    {
        private readonly IShoppingCardGetUserIdRepository _repository;

        public GetTotalQuantityByUserIdQueryHandler(IShoppingCardGetUserIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<int?> Handle(GetTotalQuantityByUserIdQuery request, CancellationToken cancellationToken)
        {
            // UserId'yi direkt repository'e iletiyoruz
            return await _repository.GetTotalQuantityByUserIdAsync(request.UserId);
        }
    }
}
