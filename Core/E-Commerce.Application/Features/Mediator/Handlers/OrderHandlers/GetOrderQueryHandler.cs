using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, List<GetOrderQueryResult>>
    {
        private readonly IRepository<Order> _repository;

        public GetOrderQueryHandler(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderQueryResult>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetOrderQueryResult
            {
                OrderID = x.OrderID,
                UserId = x.UserId,
                OrderNumber = x.OrderNumber,
                OrderPaymentAddressId = x.OrderPaymentAddressId,
                OrderPaymentCardId = x.OrderPaymentCardId,
                OrderPaymentOtherId = x.OrderPaymentOtherId,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus,
                PaymentMethod = x.PaymentMethod,
                TotalAmount = x.TotalAmount
            }).ToList();

        }
    }
}
