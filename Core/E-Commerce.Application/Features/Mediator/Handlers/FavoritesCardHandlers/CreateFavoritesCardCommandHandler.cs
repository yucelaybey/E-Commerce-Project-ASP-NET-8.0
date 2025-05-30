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
    public class CreateFavoritesCardCommandHandler : IRequestHandler<CreateFavoritesCardCommand>
    {
        private readonly IRepository<FavoritesCard> _repository;

        public CreateFavoritesCardCommandHandler(IRepository<FavoritesCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateFavoritesCardCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new FavoritesCard
            {
                UserId = request.UserId
            });
        }
    }
}
