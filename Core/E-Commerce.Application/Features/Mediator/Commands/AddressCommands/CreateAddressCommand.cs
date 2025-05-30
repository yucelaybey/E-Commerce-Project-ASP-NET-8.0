using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using ECommerce.Dto.AddressDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.AddressCommands
{
    public class CreateAddressCommand : IRequest<GetCreateResult<int>>
    {
        public CreateAddressDto CreateAddressDto { get; }

        public CreateAddressCommand(CreateAddressDto createAddressDto)
        {
            CreateAddressDto = createAddressDto;
        }
    }
}
