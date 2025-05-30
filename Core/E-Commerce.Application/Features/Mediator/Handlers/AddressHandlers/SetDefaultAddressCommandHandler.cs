using E_Commerce.Application.Features.Mediator.Commands.AddressCommands;
using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using E_Commerce.Application.Interfaces.IAddressRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AddressHandlers
{
    public class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommand, SetDefaultAddressResult>
    {
        private readonly IAddressRepository _addressRepository;

        public SetDefaultAddressCommandHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<SetDefaultAddressResult> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            var success = await _addressRepository.SetAsDefaultAddressAsync(request.AddressId, request.UserId);

            return success
                ? SetDefaultAddressResult.Success()
                : SetDefaultAddressResult.Failure("Address not found or doesn't belong to user");
        }
    }
}
