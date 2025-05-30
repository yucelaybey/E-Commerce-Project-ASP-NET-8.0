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
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, GetAddressByIdQueryResult>
    {
        private readonly IRepository<Address> _repository;

        public GetAddressByIdQueryHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<GetAddressByIdQueryResult> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetAddressByIdQueryResult
            {
                AddressID = values.AddressID,
                AddressTitle = values.AddressTitle,
                AddressDetails = values.AddressDetails,
                City = values.City,
                District = values.District,
                NameSurname = values.NameSurname,
                PhoneNumber = values.PhoneNumber,
                Town = values.Town,
                Quarter = values.Quarter,
                UserId=values.UserId,
                IsDefault = values.IsDefault
            };
        }
    }
}
