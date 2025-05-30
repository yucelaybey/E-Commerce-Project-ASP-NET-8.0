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
    public class RemoveQuarterCommandHandler : IRequestHandler<RemoveQuarterCommand>
    {
        private readonly IRepository<Quarter> _repository;

        public RemoveQuarterCommandHandler(IRepository<Quarter> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveQuarterCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
