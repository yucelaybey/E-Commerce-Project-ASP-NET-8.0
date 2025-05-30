using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IPaymentCardRepository
{
    public interface IPaymentCardRepository
    {
        Task<PaymentCard> AddAsync(PaymentCard paymentCard);
        Task<List<PaymentCard>> GetPaymentCardsByUserIdAsync(int userId);
        Task UpdatePaymentCardAsync(PaymentCard paymentCard);
        Task<PaymentCard> GetPaymentCardByIdAsync(int paymentCardId);
    }
}
