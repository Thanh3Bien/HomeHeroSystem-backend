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
    public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
    {
        public TechnicianRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public async Task<Technician?> GetTechnicianByNameAsync(string technicianName)
        {
            var technician = await _context.Technicians.FirstOrDefaultAsync(t => t.FullName == technicianName && t.IsDeleted != true);
            if (technician == null)
            {
                return null;
            }
            return technician;
        }
    }
}
