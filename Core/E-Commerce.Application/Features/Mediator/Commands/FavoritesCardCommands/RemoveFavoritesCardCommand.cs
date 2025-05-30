using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands
{
    public class RemoveFavoritesCardCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveFavoritesCardCommand(int id)
        {
            Id = id;
        }
    }
}
