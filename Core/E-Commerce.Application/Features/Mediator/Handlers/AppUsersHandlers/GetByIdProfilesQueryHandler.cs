using E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries;
using E_Commerce.Application.Features.Mediator.Results.AppUserResults;
using E_Commerce.Application.Features.Mediator.Results.ProductResults;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AppUsersHandlers
{
    public class GetByIdProfilesQueryHandler : IRequestHandler<GetByIdProfilesQuery, GetByIdProfilesQueryResult>
    {
        private readonly IRepository<AppUser> _repository;

        public GetByIdProfilesQueryHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<GetByIdProfilesQueryResult> Handle(GetByIdProfilesQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetByIdProfilesQueryResult
            {
                AppUserId = values.AppUserId,
                Email = values.Email,
                EmailCheck = values.EmailCheck,
                NameSurname = values.NameSurname,
                PhoneNumber = values.PhoneNumber,
                BirtDay = values.BirtDay,
                Password = values.Password
            };
        }
    }
}
