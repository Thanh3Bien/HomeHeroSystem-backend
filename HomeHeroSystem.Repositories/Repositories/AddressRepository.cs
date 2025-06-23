using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public async Task<Address?> FindAddressAsync(string street, string ward, string district, string city)
        {
            try
            {
                return await _dbSet
                    .FirstOrDefaultAsync(a =>
                        a.IsDeleted != true &&
                        a.Street.ToLower().Trim() == street.ToLower().Trim() &&
                        a.Ward.ToLower().Trim() == ward.ToLower().Trim() &&
                        a.District.ToLower().Trim() == district.ToLower().Trim() &&
                        a.City.ToLower().Trim() == city.ToLower().Trim());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error finding address: {street}, {ward}, {district}, {city}");
                throw;
            }
        }

        public async Task<List<string>> GetDistinctDistrictsAsync()
        {
            try
            {
                return await _dbSet
                    .Where(a => a.IsDeleted != true && !string.IsNullOrEmpty(a.District))
                    .Select(a => a.District)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting distinct districts");
                throw;
            }
        }

        public async Task<List<string>> GetWardsByDistrictAsync(string district)
        {
            try
            {
                return await _dbSet
                    .Where(a => a.IsDeleted != true &&
                               a.District == district &&
                               !string.IsNullOrEmpty(a.Ward))
                    .Select(a => a.Ward)
                    .Distinct()
                    .OrderBy(w => w)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting wards for district: {district}");
                throw;
            }
        }
    }
}
