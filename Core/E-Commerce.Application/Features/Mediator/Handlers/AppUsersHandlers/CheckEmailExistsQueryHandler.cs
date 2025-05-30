using E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries;
using E_Commerce.Application.Interfaces.AppUserInterfaces;
using ECommerce.Dto.AppUserDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AppUsersHandlers
{
    public class CheckEmailExistsQueryHandler:IRequestHandler<CheckEmailExistsQuery, CheckEmailResponseDto>
    {
        private readonly IHaveAEmailRepository _repository;

        public CheckEmailExistsQueryHandler(IHaveAEmailRepository repository)
        {
            _repository = repository;
        }

        public async Task<CheckEmailResponseDto> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.CheckEmailExistsAsync(request.Email);
        }
    }
}
