using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.FavoritesCardItemCommands
{
    public class RemoveFavoritesCardItemCommand : IRequest
    {
        public int Id { get; set; }

        public RemoveFavoritesCardItemCommand(int id)
        {
            Id = id;
        }
    }
}
