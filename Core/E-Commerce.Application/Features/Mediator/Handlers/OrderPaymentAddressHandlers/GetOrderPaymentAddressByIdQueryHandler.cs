using E_Commerce.Application.Features.Mediator.Queries.OrderPaymentAddressQueries;
using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderPaymentAddressResults;
using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentAddressHandlers
{
    public class GetOrderPaymentAddressByIdQueryHandler : IRequestHandler<GetOrderPaymentAddressByIdQuery, GetOrderPaymentAddressByIdQueryResult>
    {
        private readonly IRepository<OrderPaymentAddress> _repository;

        public GetOrderPaymentAddressByIdQueryHandler(IRepository<OrderPaymentAddress> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderPaymentAddressByIdQueryResult> Handle(GetOrderPaymentAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetOrderPaymentAddressByIdQueryResult
            {
                OrderPaymentAddressId = values.OrderPaymentAddressId,
                AddressTitle = values.AddressTitle,
                NameSurname = values.NameSurname,
                AddressDetails = values.AddressDetails,
                City = values.City,
                District = values.District,
                PhoneNumber = values.PhoneNumber,
                Quarter = values.Quarter,
                Town =values.Town
            }; ;
        }
    }
}
