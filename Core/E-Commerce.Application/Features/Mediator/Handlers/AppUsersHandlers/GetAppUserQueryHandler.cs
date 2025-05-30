using E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries;
using E_Commerce.Application.Features.Mediator.Results.AppUserResults;
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
    public class GetAppUserQueryHandler : IRequestHandler<GetAppUserQuery, List<GetAppUserQueryResult>>
    {
        private readonly IRepository<AppUser> _repository;

        public GetAppUserQueryHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetAppUserQueryResult>> Handle(GetAppUserQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetAppUserQueryResult
            {
                AppUserId = x.AppUserId,
                NameSurname = x.NameSurname,
                BirtDay = x.BirtDay,
                Email = x.Email,
                EmailCheck =x.EmailCheck,
                Password =x.Password,
                PhoneNumber = x.PhoneNumber
            }).ToList();
        }
    }
}
