using ECommerce.Dto.AppUserDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries
{
    public class CheckEmailExistsQuery : IRequest<CheckEmailResponseDto>
    {
        public string Email { get; set; }
    }
}
