using E_Commerce.Application.Features.Mediator.Commands.CategoryCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Handlers.CategoryHandlers
{
    public class RemoveCategoryCommandHander : IRequestHandler<RemoveCategoryCommand>
    {
        private readonly IRepository<Category> _repository;

        public RemoveCategoryCommandHander(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            await _repository.RemoveAsync(values);

        }
    }
}
