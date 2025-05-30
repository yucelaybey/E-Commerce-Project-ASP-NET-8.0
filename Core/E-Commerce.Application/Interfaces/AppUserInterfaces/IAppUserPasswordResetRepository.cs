using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.AppUserInterfaces
{
    public interface IAppUserPasswordResetRepository
    {
        Task<AppUser> GetByIdAsync(int id);
        Task UpdateAsync(AppUser entity);
        Task<AppUser> UpdateAppUserPasswordReset(int id, string newPassword);
    }
}
