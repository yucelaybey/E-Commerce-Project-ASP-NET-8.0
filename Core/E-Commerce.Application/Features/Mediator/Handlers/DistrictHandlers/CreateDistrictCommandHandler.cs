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
    public class CreateDistrictCommandHandler : IRequestHandler<CreateDistrictCommand>
    {
        private readonly IRepository<District> _repository;

        public CreateDistrictCommandHandler(IRepository<District> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateDistrictCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new District
            {
                DistrictName = request.DistrictName,
                CityId = request.CityId,
                QuarterId = request.QuarterId,
                TownId = request.TownId,
                PostalCode = request.PostalCode
            });
        }
    }
}
