using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentCardCommands;
using E_Commerce.Application.Interfaces.IOrderPaymentCardRepository;
using E_Commerce.Application.Interfaces.IOrderRepository;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentCardHandlers
{
    public class CreateOrderPaymentCardCommandHandler : IRequestHandler<CreateOrderPaymentCardCommand,int>
    {
        private readonly IOrderPaymentCardRepository _repository;

        public CreateOrderPaymentCardCommandHandler(IOrderPaymentCardRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateOrderPaymentCardCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderPaymentCard
            {
                OrderCardName = request.OrderCardName,
                OrderCardNumber = request.OrderCardNumber,
                OrderCardDate = request.OrderCardDate,
                OrderCardCVV = request.OrderCardCVV
            };

            return await _repository.CreateOrderPaymentCardAsync(order);
        }
    }
}