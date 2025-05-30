using E_Commerce.Application.Features.Mediator.Commands.AddressCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AddressHandlers
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
    {
        private readonly IRepository<Address> _repository;

        public UpdateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.AddressID);
            values.AddressTitle = request.AddressTitle;
            values.AddressDetails = request.AddressDetails;
            values.PhoneNumber = request.PhoneNumber;
            values.NameSurname = request.NameSurname;
            values.City = request.City;
            values.District = request.District;
            values.Town = request.Town;
            values.Quarter = request.Quarter;
            values.IsDefault = request.IsDefault;
            await _repository.UpdateAsync(values);
        }
    }
}
