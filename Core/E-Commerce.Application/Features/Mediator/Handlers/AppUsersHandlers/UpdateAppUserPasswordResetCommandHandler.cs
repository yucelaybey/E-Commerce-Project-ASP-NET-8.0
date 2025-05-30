using E_Commerce.Application.Features.Mediator.Commands.AppUserCommands;
using E_Commerce.Application.Interfaces.AppUserInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.AppUsersHandlers
{
    public class UpdateAppUserPasswordResetCommandHandler : IRequestHandler<UpdateAppUserPasswordResetCommand>
    {
        private readonly IAppUserPasswordResetRepository _repository;

        public UpdateAppUserPasswordResetCommandHandler(IAppUserPasswordResetRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateAppUserPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.AppUserId);
            values.Password = request.Password;
            await _repository.UpdateAsync(values);
        }
    }
}
