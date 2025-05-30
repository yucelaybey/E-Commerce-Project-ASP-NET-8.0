using E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands;
using E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.FavoritesHandlers
{
    public class RemoveFavoritesCardCommandHandler : IRequestHandler<RemoveFavoritesCardCommand>
    {
        private readonly IRepository<FavoritesCard> _repository;

        public RemoveFavoritesCardCommandHandler(IRepository<FavoritesCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveFavoritesCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
