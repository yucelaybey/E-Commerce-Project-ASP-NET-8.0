using ECommerce.Dto.PaymentCardDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries
{
    public class GetUserPaymentCardsResult : IRequest<List<GetPaymentCardDto>>
    {
        public int UserId { get; set; }
    }
}
