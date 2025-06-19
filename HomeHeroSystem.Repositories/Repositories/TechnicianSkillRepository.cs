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
    public class TechnicianSkillRepository : GenericRepository<TechnicianSkill>, ITechnicianSkillRepository
    {
        public TechnicianSkillRepository(HomeHeroContext context, ILogger logger)
        : base(context, logger)
        {
        }
        public async Task<bool> IsSkillExistAsync(int skillId)
        {
            try
            {
                return await _context.Skills
                    .AnyAsync(s => s.SkillId == skillId && s.IsDeleted != true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking skill existence");
                throw;
            }
        }

        public async Task<List<TechnicianSkill>> GetByTechnicianIdAsync(int technicianId)
        {
            try
            {
                return await _dbSet
                    .Where(ts => ts.TechnicianId == technicianId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting technician skills");
                throw;
            }
        }

        public async Task DeleteByTechnicianIdAsync(int technicianId)
        {
            try
            {
                var existingSkills = await GetByTechnicianIdAsync(technicianId);
                _dbSet.RemoveRange(existingSkills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting technician skills");
                throw;
            }
        }
    }
}
