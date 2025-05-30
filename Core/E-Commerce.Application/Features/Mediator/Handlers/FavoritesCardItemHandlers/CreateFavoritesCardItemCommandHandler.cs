using E_Commerce.Application.Features.Mediator.Commands.FavoritesCardItemCommands;
using E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.IFavoritesCardItemRepository;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesCardItemHandlers
{
    public class CreateFavoritesCardItemCommandHandler : IRequestHandler<CreateFavoritesCardItemCommand, FavoritesCardItem>
    {
        private readonly IFavoritesCardItemRepository _repository;

        public CreateFavoritesCardItemCommandHandler(IFavoritesCardItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<FavoritesCardItem> Handle(CreateFavoritesCardItemCommand request, CancellationToken cancellationToken)
        {
            var item = new FavoritesCardItem
            {
                FavoritesCardID = request.FavoritesCardID,
                ProductID = request.ProductID
            };

            return await _repository.AddAsync(item);
        }
    }
}
