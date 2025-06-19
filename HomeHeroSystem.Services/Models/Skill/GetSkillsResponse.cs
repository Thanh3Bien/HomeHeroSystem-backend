using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Skill
{
    public class GetSkillsResponse
    {
        public List<SkillResponse> Skills { get; set; } = new();
    }
}
