using E_Commerce.Application.Features.Mediator.Commands.ProductCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.ProductHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IRepository<Product> _repository;

        public CreateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new Product
            {
                Name = request.Name,
                CategoryID = request.CategoryID,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Price = request.Price,
                SalePrice = request.SalePrice,
                DiscountRate = request.DiscountRate,
                Stock = request.Stock,
                SaleSeason = request.SaleSeason

            });
        }
    }
}
