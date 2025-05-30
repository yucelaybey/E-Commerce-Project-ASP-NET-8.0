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
    public class RemoveDistrictCommandHandler : IRequestHandler<RemoveTownCommand>
    {
        private readonly IRepository<Town> _repository;

        public RemoveDistrictCommandHandler(IRepository<Town> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveTownCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
