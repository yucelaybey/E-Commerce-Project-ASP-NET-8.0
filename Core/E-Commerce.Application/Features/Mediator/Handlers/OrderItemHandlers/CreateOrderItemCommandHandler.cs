using E_Commerce.Application.Features.Mediator.Commands.OrderItemCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderItemHandlers
{
    public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand>
    {
        private readonly IRepository<OrderItem> _repository;

        public CreateOrderItemCommandHandler(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new OrderItem
            {
                OrderID = request.OrderID,
                ProductID = request.ProductID,
                Quantity = request.Quantity,
                Price = request.Price,
                OrderStatus = request.OrderStatus,
                Description = request.Description,
                Name = request.Name,
                SalePrice = request.SalePrice,
                Stock = request.Stock,
                DiscountRate = request.DiscountRate,
                ImageUrl = request.ImageUrl,
                CategoryID = request.CategoryID,
                OrderDate = request.OrderDate,
                SaleSeason = request.SaleSeason
            });
        }
    }
}
