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
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(HomeHeroContext context, ILogger logger)
        : base(context, logger)
        {
        }

        public async Task<List<Skill>> GetActiveSkillsAsync()
        {
            return await _dbSet
                .Where(s => s.IsDeleted != true)
                .OrderBy(s => s.SkillName)
                .ToListAsync();
        }
    }
}
