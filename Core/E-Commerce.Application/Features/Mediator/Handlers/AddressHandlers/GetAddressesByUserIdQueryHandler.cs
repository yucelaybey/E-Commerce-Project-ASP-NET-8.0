using E_Commerce.Application.Features.Mediator.Queries.AddressQueries;
using E_Commerce.Application.Interfaces.IAddressRepository;
using ECommerce.Dto.AddressDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AddressHandlers
{
    public class GetAddressesByUserIdQueryHandler : IRequestHandler<GetAddressesByUserIdQuery, List<GetAddressDto>>
    {
        private readonly IAddressRepository _repository;

        public GetAddressesByUserIdQueryHandler(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetAddressDto>> Handle(GetAddressesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _repository.GetAddressesByUserIdAsync(request.UserId);

            return addresses.Select(address => new GetAddressDto
            {
                AddressID = address.AddressID,
                UserId = address.UserId,
                AddressTitle = address.AddressTitle,
                AddressDetails = address.AddressDetails,
                NameSurname = address.NameSurname,
                PhoneNumber = address.PhoneNumber,
                City = address.City,
                Town = address.Town,
                Quarter = address.Quarter,
                District = address.District,
                IsDefault = address.IsDefault
            }).ToList();
        }
    }
}
