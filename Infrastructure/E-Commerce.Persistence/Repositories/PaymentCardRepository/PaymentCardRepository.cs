using E_Commerce.Application.Interfaces.IPaymentCardRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.PaymentCardRepository
{
    public class PaymentCardRepository : IPaymentCardRepository
    {
        private readonly ECommerceContext _context;

        public PaymentCardRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<PaymentCard> AddAsync(PaymentCard paymentCard)
        {
            await _context.PaymentCards.AddAsync(paymentCard);
            await _context.SaveChangesAsync();
            return paymentCard;
        }

        public async Task<List<PaymentCard>> GetPaymentCardsByUserIdAsync(int userId)
        {
            return await _context.PaymentCards
                .Where(pc => pc.UserId == userId)
                .ToListAsync();
        }
        public async Task UpdatePaymentCardAsync(PaymentCard paymentCard)
        {
            _context.PaymentCards.Update(paymentCard);
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentCard> GetPaymentCardByIdAsync(int paymentCardId)
        {
            return await _context.PaymentCards
                .FirstOrDefaultAsync(pc => pc.PaymentCardID == paymentCardId);
        }
    }
}
