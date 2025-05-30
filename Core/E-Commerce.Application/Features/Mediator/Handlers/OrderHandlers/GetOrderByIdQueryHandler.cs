using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using E_Commerce.Application.Features.Mediator.Results.OrderResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderHandlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdQueryResult>
    {
        private readonly IRepository<Order> _repository;

        public GetOrderByIdQueryHandler(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderByIdQueryResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetOrderByIdQueryResult
            {
                OrderID = values.OrderID,
                UserId = values.UserId,
                OrderNumber = values.OrderNumber,
                OrderPaymentAddressId = values.OrderPaymentAddressId,
                OrderPaymentCardId = values.OrderPaymentCardId,
                OrderPaymentOtherId= values.OrderPaymentOtherId,
                OrderDate = values.OrderDate,
                OrderStatus = values.OrderStatus,
                PaymentMethod = values.PaymentMethod,
                TotalAmount = values.TotalAmount
            }; ;
        }
    }
}
