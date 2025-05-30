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
    public class UpdateFavoritesCardCommandHandler : IRequestHandler<UpdateFavoritesCardCommand>
    {
        private readonly IRepository<FavoritesCard> _repository;

        public UpdateFavoritesCardCommandHandler(IRepository<FavoritesCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateFavoritesCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.FavoritesCardID);
            values.UserId = request.UserId;
            await _repository.UpdateAsync(values);
        }
    }
}
