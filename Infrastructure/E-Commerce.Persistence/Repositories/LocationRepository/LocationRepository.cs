using E_Commerce.Application.Interfaces.ILocationRepository;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories.LocationRepository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ECommerceContext _context;

        public LocationRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<List<Town>> GetTownsByCityIdAsync(int cityId)
        {
            return await _context.Towns
                .Where(t => t.CityId == cityId)
                .ToListAsync();
        }

        public async Task<List<Quarter>> GetQuartersByTownIdAsync(int townId)
        {
            return await _context.Quarters
                .Where(q => q.TownId == townId)
                .ToListAsync();
        }

        public async Task<List<District>> GetDistrictsByQuarterIdAsync(int quarterId)
        {
            return await _context.Districts
                .Where(d => d.QuarterId == quarterId)
                .ToListAsync();
        }
    }
}
