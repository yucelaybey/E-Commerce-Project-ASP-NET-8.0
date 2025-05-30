using E_Commerce.Application.Features.Mediator.Commands.ProductCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ProductHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.ProductID);
            values.Name = request.Name;
            values.Stock = request.Stock;
            values.Price = request.Price;
            values.CategoryID = request.CategoryID;
            values.Description = request.Description;
            values.ImageUrl = request.ImageUrl;
            values.SalePrice = request.SalePrice;
            values.DiscountRate = request.DiscountRate;
            values.SaleSeason = request.SaleSeason;
            await _repository.UpdateAsync(values);
        }
    }
}
