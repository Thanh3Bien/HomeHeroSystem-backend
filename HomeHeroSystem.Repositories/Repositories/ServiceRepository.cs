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
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public async Task<Service?> GetServiceByNameAsync(string serviceName)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceName == serviceName && s.IsDeleted !=true);
            return service;
        }
        public async Task<IEnumerable<string>> GetServiceNamesAsync()
        {
            // Example implementation
            return await _context.Services
                .Where(s => s.IsActive == true)
                .Select(s => s.ServiceName)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> SearchServiceNamesAsync(string keyword)
        {
            return await _context.Services
                .Where(s => s.IsActive == true && s.ServiceName.Contains(keyword))
                .Select(s => s.ServiceName)
                .Distinct()
                .ToListAsync();
        }


        public async Task<IEnumerable<Service>> GetActiveByCategoryIdAsync(int categoryId)
        {
            return await _context.Services
                .Include(s => s.Category)
                .Where(s => s.CategoryId == categoryId && s.IsActive == true && s.IsDeleted != true)
                .OrderBy(s => s.Price)
                .ToListAsync();
        }

        public async Task<decimal> GetServicePriceAsync(int serviceId)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId == true && s.IsDeleted != true);

            return service?.Price ?? 0;
        }
    }
}
