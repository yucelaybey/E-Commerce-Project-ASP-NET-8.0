using E_Commerce.Application.Features.Mediator.Commands.AddressCommands;
using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.IAddressRepository;
using E_Commerce.Domain.Entities;
using MediatR;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AddressHandlers
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, GetCreateResult<int>>
    {
        private readonly IAddressRepository _addressRepository;

        public CreateAddressCommandHandler(
            IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<GetCreateResult<int>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var address = new Address
                {
                    UserId = request.CreateAddressDto.UserId,
                    AddressTitle = request.CreateAddressDto.AddressTitle,
                    AddressDetails = request.CreateAddressDto.AddressDetails,
                    NameSurname = request.CreateAddressDto.NameSurname,
                    PhoneNumber = request.CreateAddressDto.PhoneNumber,
                    City = request.CreateAddressDto.City,
                    Town = request.CreateAddressDto.Town,
                    Quarter = request.CreateAddressDto.Quarter,
                    District = request.CreateAddressDto.District,
                    IsDefault = request.CreateAddressDto.IsDefault
                };

                // Eğer bu adres varsayılan olarak işaretlendiyse, diğer adreslerin varsayılanlığını kaldır
                if (address.IsDefault)
                {
                    await _addressRepository.RemoveDefaultAddressesAsync(address.UserId);
                }

                var addressId = await _addressRepository.AddAsync(address);

                return GetCreateResult<int>.SuccessResult(addressId, "Adres başarıyla eklendi");
            }
            catch (Exception ex)
            {
                return GetCreateResult<int>.FailureResult("Adres eklenirken bir hata oluştu");
            }
        }
    }
}
