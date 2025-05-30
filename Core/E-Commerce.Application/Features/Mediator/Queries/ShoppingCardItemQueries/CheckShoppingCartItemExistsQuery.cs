using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries
{
    public class CheckShoppingCartItemExistsQuery : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int ShoppingCardId { get; set; }
    }
}
