using E_Commerce.Application.Features.Mediator.Commands.ProductCommands;
using E_Commerce.Application.Interfaces.IProductRepository;
using ECommerce.Dto.ProductDtos;
using MediatR;

namespace ECommerce.Application.Features.Mediator.Handlers.ProductHandlers
{
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, UpdateProductStockResponseDto>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductStockCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<UpdateProductStockResponseDto> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new UpdateProductStockResponseDto
                {
                    IsSuccess = false,
                    Message = "Ürün bulunamadı!"
                };
            }

            var oldStock = product.Stock;
            product.Stock = request.NewStock;
            await _productRepository.UpdateAsync(product);

            return new UpdateProductStockResponseDto
            {
                ProductId = request.ProductId,
                OldStock = oldStock,
                NewStock = request.NewStock,
                IsSuccess = true,
                Message = "Stok güncelleme başarılı!"
            };
        }
    }
}