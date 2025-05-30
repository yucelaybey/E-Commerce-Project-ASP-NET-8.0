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
    public class CreateQuarterCommandHandler : IRequestHandler<CreateQuarterCommand>
    {
        private readonly IRepository<Quarter> _repository;

        public CreateQuarterCommandHandler(IRepository<Quarter> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateQuarterCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new Quarter
            {
                QuarterName = request.QuarterName,
                CityId = request.CityId,
                TownId = request.TownId
            });
        }
    }
}
