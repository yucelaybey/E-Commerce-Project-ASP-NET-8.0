using E_Commerce.Application.Features.Mediator.Commands.CityCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.CityHandlers
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
    {
        private readonly IRepository<City> _repository;

        public UpdateCityCommandHandler(IRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.CityId);
            values.CityName = request.CityName;
            await _repository.UpdateAsync(values);
        }
    }
}
