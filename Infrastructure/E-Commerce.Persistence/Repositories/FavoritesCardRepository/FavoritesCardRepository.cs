using E_Commerce.Application.Interfaces.IFavoritesCardRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.FavoritesCardRepository
{
    public class FavoritesCardRepository : IFavoritesCardRepository
    {
        private readonly ECommerceContext _context;

        public FavoritesCardRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<FavoritesCard>> GetByUserIdAsync(int userId)
        {
            return await _context.FavoritesCards
                .Where(fc => fc.UserId == userId)
                .ToListAsync();
        }
    }
}
