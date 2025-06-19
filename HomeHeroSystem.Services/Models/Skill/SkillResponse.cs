using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Skill
{
    public class SkillResponse
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
