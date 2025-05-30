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
    public class UpdateDistrictCommandHandler : IRequestHandler<UpdateTownCommand>
    {
        private readonly IRepository<Town> _repository;

        public UpdateDistrictCommandHandler(IRepository<Town> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateTownCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.TownId);
            values.CityId = request.CityId;
            values.TownName = request.TownName;
            await _repository.UpdateAsync(values);
        }
    }
}
