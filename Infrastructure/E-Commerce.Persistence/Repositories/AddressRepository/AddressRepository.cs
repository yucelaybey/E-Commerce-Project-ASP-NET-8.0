using E_Commerce.Application.Interfaces.IAddressRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.AddressRepository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ECommerceContext _context;

        public AddressRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> SetAsDefaultAddressAsync(int addressId, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Kullanıcının tüm adreslerini getir
                var userAddresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                // 2. Seçilen adresin kullanıcıya ait olduğunu kontrol et
                var selectedAddress = userAddresses.FirstOrDefault(a => a.AddressID == addressId);
                if (selectedAddress == null)
                    return false;

                // 3. Tüm adresleri false yap
                foreach (var address in userAddresses)
                {
                    address.IsDefault = false;
                }

                // 4. Seçilen adresi true yap
                selectedAddress.IsDefault = true;

                // 5. Değişiklikleri kaydet
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDefaultAddressesAsync(int userId)
        {
            var defaultAddresses = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault)
                .ToListAsync();

            foreach (var address in defaultAddresses)
            {
                address.IsDefault = false;
            }
        }

        public async Task<int> AddAsync(Address address)
        {
            var entity = await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return entity.Entity.AddressID;
        }
    }
}
