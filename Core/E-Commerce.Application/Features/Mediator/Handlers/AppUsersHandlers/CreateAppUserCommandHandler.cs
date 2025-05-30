using E_Commerce.Application.Enums;
using E_Commerce.Application.Features.Mediator.Commands.AppUserCommands;
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
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand>
    {
        private readonly IRepository<AppUser> _repository;

        public CreateAppUserCommandHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new AppUser
            {
                Password = request.Password,
                Email = request.Email,
                AppRoleId = (int)RolesType.Member,
                NameSurname = request.NameSurname,
                BirtDay = request.BirthDay,
                EmailCheck = request.EmailConfirm,
                PhoneNumber = request.PhoneNumber
            });
        }
    }
}
