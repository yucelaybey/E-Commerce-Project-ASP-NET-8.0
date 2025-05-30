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
    public class UpdateOrderPaymentOtherCommandHandler : IRequestHandler<UpdateOrderPaymentOtherCommand>
    {
        private readonly IRepository<OrderPaymentOther> _repository;

        public UpdateOrderPaymentOtherCommandHandler(IRepository<OrderPaymentOther> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderPaymentOtherCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderPaymentOtherId);
            values.PaymentName = request.PaymentName;
            values.PaymentNumber = request.PaymentNumber;
            await _repository.UpdateAsync(values);
        }
    }
}
