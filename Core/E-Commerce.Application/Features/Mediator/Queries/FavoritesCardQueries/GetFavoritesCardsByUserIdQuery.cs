using ECommerce.Dto.FavoritesCardDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.FavoritesCardQueries
{
    public class GetFavoritesCardsByUserIdQuery : IRequest<List<FavoritesCardDto>>
    {
        public int UserId { get; set; }
    }
}
