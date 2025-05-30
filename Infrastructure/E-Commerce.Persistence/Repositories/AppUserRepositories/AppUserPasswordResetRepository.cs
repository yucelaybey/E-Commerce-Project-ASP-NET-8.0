using E_Commerce.Application.Interfaces.AppUserInterfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.AppUserRepositories
{
    public class AppUserPasswordResetRepository : IAppUserPasswordResetRepository
    {
        private readonly ECommerceContext _context;

        public AppUserPasswordResetRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _context.Set<AppUser>().FindAsync(id);
        }

        public async Task<AppUser> UpdateAppUserPasswordReset(int id, string newPassword)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException("Kullanıcı bulunamadı");
            }

            user.Password = newPassword;

            // Değişiklikleri kaydet
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(AppUser entity)
        {
            _context.Set<AppUser>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
