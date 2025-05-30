using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentCardCommands;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Handlers.OrderPaymentCardHandlers
{
    public class UpdateOrderPaymentCardCommandHandler : IRequestHandler<UpdateOrderPaymentCardCommand>
    {
        private readonly IRepository<OrderPaymentCard> _repository;

        public UpdateOrderPaymentCardCommandHandler(IRepository<OrderPaymentCard> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderPaymentCardCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderPaymentCardId);
            values.OrderCardName = request.OrderCardName;
            values.OrderCardNumber = request.OrderCardNumber;
            values.OrderCardDate = request.OrderCardDate;
            values.OrderCardCVV = request.OrderCardCVV;
            await _repository.UpdateAsync(values);
        }
    }
}
