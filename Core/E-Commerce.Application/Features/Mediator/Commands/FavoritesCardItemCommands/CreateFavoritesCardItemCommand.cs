using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.FavoritesCardItemCommands
{
    public class CreateFavoritesCardItemCommand : IRequest<FavoritesCardItem>
    {
        public int FavoritesCardID { get; set; }
        public int ProductID { get; set; }

        public CreateFavoritesCardItemCommand(int favoritesCardID, int productID)
        {
            FavoritesCardID = favoritesCardID;
            ProductID = productID;
        }
    }
}
