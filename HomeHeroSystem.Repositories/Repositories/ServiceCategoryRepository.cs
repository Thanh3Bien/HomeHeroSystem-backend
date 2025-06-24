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
    public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(HomeHeroContext context, ILogger logger)
           : base(context, logger)
        {
        }
        public async Task<IEnumerable<ServiceCategory>> GetActiveAsync()
        {
            return await _context.ServiceCategories
                .Where(sc => sc.IsActive == true && sc.IsDeleted != true)
                .OrderBy(sc => sc.CategoryName)
                .ToListAsync();
        }
    }
}
