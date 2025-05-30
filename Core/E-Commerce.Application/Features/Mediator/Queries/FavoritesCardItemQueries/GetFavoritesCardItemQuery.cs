using E_Commerce.Application.Features.Mediator.Results.FavoritesCardItemResults;
using E_Commerce.Application.Features.Mediator.Results.FavoritesCardResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries
{
    public class GetFavoritesCardItemQuery : IRequest<List<GetFavoritesCardItemQueryResult>>
    {
    }
}
