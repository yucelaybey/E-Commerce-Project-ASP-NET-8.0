using E_Commerce.Application.Features.Mediator.Commands.AppUserCommands;
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
    public class UpdateProfilesCommandHandler : IRequestHandler<UpdateByIdProfilesCommand>
    {
        private readonly IRepository<AppUser> _repository;

        public UpdateProfilesCommandHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateByIdProfilesCommand request, CancellationToken cancellationToken)
        {
            var value = await _repository.GetByIdAsync(request.AppUserId);
            value.NameSurname = request.NameSurname;
            value.Email = request.Email;
            value.EmailCheck = request.EmailCheck;
            value.PhoneNumber = request.PhoneNumber;
            value.BirtDay = request.BirtDay;
            await _repository.UpdateAsync(value);
        }
    }
}
