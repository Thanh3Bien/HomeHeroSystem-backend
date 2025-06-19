using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class TechnicianSkillRequest
    {
        public int SkillId { get; set; }    
        public int ProficiencyLevel { get; set; }
        public int? YearsOfExperience { get; set; }
    }
}
