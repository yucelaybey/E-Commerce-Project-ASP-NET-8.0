using ECommerce.Dto.AppUserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.AppUserInterfaces
{
    public interface IHaveAEmailRepository
    {
        Task<CheckEmailResponseDto> CheckEmailExistsAsync(string email);
    }
}
