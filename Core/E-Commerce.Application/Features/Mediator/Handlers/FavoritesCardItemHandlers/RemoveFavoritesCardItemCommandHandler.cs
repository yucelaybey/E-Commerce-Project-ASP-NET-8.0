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
    public class RemoveFavoritesCardItemCommandHandler : IRequestHandler<RemoveFavoritesCardItemCommand>
    {
        private readonly IRepository<FavoritesCardItem> _repository;

        public RemoveFavoritesCardItemCommandHandler(IRepository<FavoritesCardItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveFavoritesCardItemCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
