using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentOrderCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.IOrderPaymentOtherRepository;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentOtherHandlers
{
    public class CreateOrderPaymentOtherCommandHandler : IRequestHandler<CreateOrderPaymentOtherCommand,int>
    {
        private readonly IOrderPaymentOtherRepository _repository;

        public CreateOrderPaymentOtherCommandHandler(IOrderPaymentOtherRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateOrderPaymentOtherCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderPaymentOther
            {
                PaymentName = request.PaymentName,
                PaymentNumber = request.PaymentNumber
            };

            return await _repository.CreateOrderPaymentOtherAsync(order);
        }
    }
}
