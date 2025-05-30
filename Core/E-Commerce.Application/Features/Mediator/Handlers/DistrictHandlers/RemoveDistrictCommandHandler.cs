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
    public class RemoveDistrictCommandHandler : IRequestHandler<RemoveDistrictCommand>
    {
        private readonly IRepository<District> _repository;

        public RemoveDistrictCommandHandler(IRepository<District> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveDistrictCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
