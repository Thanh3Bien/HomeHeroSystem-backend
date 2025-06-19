using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface ITechnicianSkillRepository : IGenericRepository<TechnicianSkill>
    {
        Task<bool> IsSkillExistAsync(int skillId);

        Task<List<TechnicianSkill>> GetByTechnicianIdAsync(int technicianId);
        Task DeleteByTechnicianIdAsync(int technicianId);
    }
}
