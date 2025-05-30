using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands
{
    public class UpdateFavoritesCardCommand : IRequest
    {
        public int FavoritesCardID { get; set; }
        public int UserId { get; set; }
    }
}
