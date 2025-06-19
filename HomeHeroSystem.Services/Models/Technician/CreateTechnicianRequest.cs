using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class CreateTechnicianRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int? ExperienceYears { get; set; }
        public int AddressId { get; set; } 
        public List<TechnicianSkillRequest> Skills { get; set; } = new List<TechnicianSkillRequest>();
    }
}
