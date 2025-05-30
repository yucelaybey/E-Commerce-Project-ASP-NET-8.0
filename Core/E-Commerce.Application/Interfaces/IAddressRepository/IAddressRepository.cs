using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IAddressRepository
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAddressesByUserIdAsync(int userId);
        Task<bool> SetAsDefaultAddressAsync(int addressId, int userId);
        Task<int> AddAsync(Address address);
        Task SaveChangesAsync();
        Task RemoveDefaultAddressesAsync(int userId);

    }
}
