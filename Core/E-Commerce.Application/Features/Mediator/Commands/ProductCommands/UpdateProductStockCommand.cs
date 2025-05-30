using ECommerce.Dto.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.ProductCommands
{
    public class UpdateProductStockCommand : IRequest<UpdateProductStockResponseDto>
    {
        public int ProductId { get; set; }
        public int NewStock { get; set; }
    }
}
