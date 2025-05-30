using E_Commerce.Application.Features.Mediator.Commands.FavoritesCardItemCommands;
using E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesCardItemHandlers
{
    public class UpdateFavoritesCardItemCommandHandler : IRequestHandler<UpdateFavoritesCardItemCommand>
    {
        private readonly IRepository<FavoritesCardItem> _repository;

        public UpdateFavoritesCardItemCommandHandler(IRepository<FavoritesCardItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateFavoritesCardItemCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.FavoritesCardItemID);
            values.FavoritesCardID = request.FavoritesCardID;
            values.ProductID = request.ProductID;
            await _repository.UpdateAsync(values);
        }
    }
}
