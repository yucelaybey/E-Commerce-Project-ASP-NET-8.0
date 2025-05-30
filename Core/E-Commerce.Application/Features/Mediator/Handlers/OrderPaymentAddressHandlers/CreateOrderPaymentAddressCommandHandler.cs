using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentAddressCommands;
using E_Commerce.Application.Interfaces.IOrderPaymentAddressRepository;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentAddressHandlers
{
    public class CreateOrderPaymentAddressCommandHandler : IRequestHandler<CreateOrderPaymentAddressCommand, int>
    {
        private readonly IOrderPaymentAddressRepository _repository;

        public CreateOrderPaymentAddressCommandHandler(IOrderPaymentAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateOrderPaymentAddressCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderPaymentAddress
            {
               AddressTitle = request.AddressTitle,
               NameSurname = request.NameSurname,
               PhoneNumber = request.PhoneNumber,
               City = request.City,
               Town = request.Town,
               Quarter = request.Quarter,
               District = request.District,
               AddressDetails = request.AddressDetails
            };

            return await _repository.CreateOrderPaymentAddressAsync(order);
        }
    }
}
