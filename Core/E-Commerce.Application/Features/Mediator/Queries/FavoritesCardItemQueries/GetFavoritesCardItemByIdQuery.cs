using E_Commerce.Application.Features.Mediator.Results.FavoritesCardItemResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries
{
    public class GetFavoritesCardItemByIdQuery : IRequest<GetFavoritesCardItemByIdQueryResult>
    {
        public int Id { get; set; }

        public GetFavoritesCardItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}
