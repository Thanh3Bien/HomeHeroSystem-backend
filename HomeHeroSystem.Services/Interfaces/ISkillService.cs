using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Skill;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface ISkillService
    {
        Task<GetSkillsResponse> GetSkillsAsync();
    }
}
