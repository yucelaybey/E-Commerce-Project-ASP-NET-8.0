using E_Commerce.Application.Interfaces.AppUserInterfaces;
using E_Commerce.Persistence.Context;
using ECommerce.Dto.AppUserDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.AppUserRepositories
{
    public class HaveAEmailRepository : IHaveAEmailRepository
    {
        private readonly ECommerceContext _context;

        public HaveAEmailRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<CheckEmailResponseDto> CheckEmailExistsAsync(string email)
        {
            var user = await _context.AppUsers
                .Where(u => u.Email == email)
                .Select(u => new CheckEmailResponseDto
                {
                    Id=u.AppUserId,
                    EmailExists = true,
                    NameSurname = u.NameSurname,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new CheckEmailResponseDto
                {
                    Id = 0,
                    EmailExists = false,
                    NameSurname = null,
                    Email = null
                };
            }

            return user;
        }
    }
}
