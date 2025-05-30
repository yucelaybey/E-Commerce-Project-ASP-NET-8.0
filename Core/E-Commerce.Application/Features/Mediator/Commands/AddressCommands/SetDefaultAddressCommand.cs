using E_Commerce.Application.Features.Mediator.Results.AddressResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Commands.AddressCommands
{
    public class SetDefaultAddressCommand : IRequest<SetDefaultAddressResult>
    {
        public int AddressId { get; set; }  // Hangi adresin default yapılacağı
        public int UserId { get; set; }   // Hangi kullanıcının adresleri güncellenecek
    }
}
