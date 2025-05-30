using E_Commerce.Application.Features.Mediator.Commands.TownCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.TownHandlers
{
    public class CreateDistrictCommandHandler : IRequestHandler<CreateTownCommand>
    {
        private readonly IRepository<Town> _repository;

        public CreateDistrictCommandHandler(IRepository<Town> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateTownCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new Town
            {
                TownName = request.TownName,
                CityId = request.CityId
            });
        }
    }
}
