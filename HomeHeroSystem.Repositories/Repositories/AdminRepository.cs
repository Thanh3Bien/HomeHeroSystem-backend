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
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Admin>> GetAllAsync()
        {
            try
            {
                return await _dbSet
                    .Where(a => a.IsActive == true)
                    .OrderBy(a => a.FullName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all admins");
                throw;
            }
        }
    }
}
