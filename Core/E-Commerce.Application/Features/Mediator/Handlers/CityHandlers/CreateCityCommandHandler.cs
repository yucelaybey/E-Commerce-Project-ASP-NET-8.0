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
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand>
    {
        private readonly IRepository<City> _repository;

        public CreateCityCommandHandler(IRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new City
            {
                CityName = request.CityName
            });
        }
    }
}
