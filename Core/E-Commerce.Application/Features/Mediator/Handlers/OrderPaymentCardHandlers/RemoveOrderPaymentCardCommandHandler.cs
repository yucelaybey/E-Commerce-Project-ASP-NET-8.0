using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentCardHandlers
{
    public class RemoveOrderPaymentCardCommandHandler : IRequestHandler<RemoveOrderPaymentCardCommand>
    {
        private readonly IRepository<OrderPaymentCard> _repository;

        public RemoveOrderPaymentCardCommandHandler(IRepository<OrderPaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveOrderPaymentCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);
        }
    }
}
