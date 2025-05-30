using E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries;
using E_Commerce.Application.Features.Mediator.Results.AppUserResults;
using E_Commerce.Application.Features.Mediator.Results.CategoryResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.AppUserInterfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AppUsersHandlers
{
    public class GetAppUserByIdQueryHandler : IRequestHandler<GetAppUserByIdQuery, GetAppUserByIdQueryResult>
    {
        private readonly IAppUserRepository _repository;

        public GetAppUserByIdQueryHandler(IAppUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAppUserByIdQueryResult> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByEmailAsync(request.Email);
            return new GetAppUserByIdQueryResult
            {
                AppUserId = values.AppUserId,
                Email = values.Email,
                NameSurname = values.NameSurname
            };
        }
    }
}
