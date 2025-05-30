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
    public class RemoveCityCommandHandler : IRequestHandler<RemoveCityCommand>
    {
        private readonly IRepository<City> _repository;

        public RemoveCityCommandHandler(IRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveCityCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
