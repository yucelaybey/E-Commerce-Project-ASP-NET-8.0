using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.ILocationRepository
{
    public interface ILocationRepository
    {
        Task<List<Town>> GetTownsByCityIdAsync(int cityId);
        Task<List<Quarter>> GetQuartersByTownIdAsync(int townId);
        Task<List<District>> GetDistrictsByQuarterIdAsync(int quarterId);
    }
}
