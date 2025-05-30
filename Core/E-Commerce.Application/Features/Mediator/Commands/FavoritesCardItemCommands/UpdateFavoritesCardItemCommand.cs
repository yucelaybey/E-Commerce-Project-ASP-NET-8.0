using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.FavoritesCardItemCommands
{
    public class UpdateFavoritesCardItemCommand : IRequest
    {
        public int FavoritesCardItemID { get; set; }
        public int FavoritesCardID { get; set; }
        public int ProductID { get; set; }
    }
}
