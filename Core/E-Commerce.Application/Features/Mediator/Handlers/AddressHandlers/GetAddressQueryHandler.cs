using E_Commerce.Application.Features.Mediator.Queries.AddressQueries;
using E_Commerce.Application.Features.Mediator.Results.AddressResults;
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
    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, List<GetAddressQueryResult>>
    {
        private readonly IRepository<Address> _repository;

        public GetAddressQueryHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetAddressQueryResult>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetAddressQueryResult
            {
                AddressID = x.AddressID,
                AddressDetails = x.AddressDetails,
                AddressTitle = x.AddressTitle,
                UserId = x.UserId,
                Town = x.Town,
                PhoneNumber = x.PhoneNumber,
                NameSurname = x.NameSurname,
                District = x.District,
                City = x.City,
                Quarter = x.Quarter,
                IsDefault = x.IsDefault
            }).ToList();
        }
    }
}
