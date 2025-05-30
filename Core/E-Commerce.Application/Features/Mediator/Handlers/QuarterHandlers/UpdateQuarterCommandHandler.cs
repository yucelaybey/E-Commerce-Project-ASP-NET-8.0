using E_Commerce.Application.Features.Mediator.Commands.QuarterCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.QuarterHandlers
{
    public class UpdateQuarterCommandHandler : IRequestHandler<UpdateQuarterCommand>
    {
        private readonly IRepository<Quarter> _repository;

        public UpdateQuarterCommandHandler(IRepository<Quarter> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateQuarterCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.QuarterId);
            values.QuarterName = request.QuarterName;
            values.CityId = request.CityId;
            values.TownId = request.TownId;
            await _repository.UpdateAsync(values);
        }
    }
}
