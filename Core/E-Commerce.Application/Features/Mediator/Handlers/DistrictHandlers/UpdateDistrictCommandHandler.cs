using E_Commerce.Application.Features.Mediator.Commands.DistrictCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.DistrictHandlers
{
    public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand>
    {
        private readonly IRepository<District> _repository;

        public UpdateDistrictCommandHandler(IRepository<District> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.DistrictId);
            values.CityId = request.CityId;
            values.TownId = request.TownId;
            values.QuarterId = request.QuarterId;
            values.DistrictName = request.DistrictName;
            values.PostalCode = request.PostalCode;
            await _repository.UpdateAsync(values);
        }
    }
}
