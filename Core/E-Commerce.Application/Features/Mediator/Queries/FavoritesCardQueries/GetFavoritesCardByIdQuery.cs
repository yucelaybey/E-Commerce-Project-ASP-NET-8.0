using E_Commerce.Application.Features.Mediator.Results.FavoritesCardResults;
using E_Commerce.Application.Features.Mediator.Results.PaymentCardResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.FavoritesQueries
{
    public class GetFavoritesCardByIdQuery : IRequest<GetFavoritesCardByIdQueryResult>
    {
        public int Id { get; set; }

        public GetFavoritesCardByIdQuery(int id)
        {
            Id = id;
        }
    }
}
