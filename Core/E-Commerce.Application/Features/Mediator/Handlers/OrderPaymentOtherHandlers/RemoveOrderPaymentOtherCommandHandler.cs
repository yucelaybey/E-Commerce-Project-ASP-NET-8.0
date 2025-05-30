using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentOrderCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentOtherHandlers
{
    public class RemoveOrderPaymentOtherCommandHandler : IRequestHandler<RemoveOrderPaymentOtherCommand>
    {
        private readonly IRepository<OrderPaymentOther> _repository;

        public RemoveOrderPaymentOtherCommandHandler(IRepository<OrderPaymentOther> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveOrderPaymentOtherCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
