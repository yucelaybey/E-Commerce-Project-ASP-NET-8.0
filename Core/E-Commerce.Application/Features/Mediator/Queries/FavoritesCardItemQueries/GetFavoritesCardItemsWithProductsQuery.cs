using ECommerce.Dto.FavoritesCardItemDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries
{
    public class GetFavoritesCardItemsWithProductsQuery : IRequest<List<FavoritesCardItemWithProductDto>>
    {
        public int FavoritesCardId { get; set; }
    }
}
