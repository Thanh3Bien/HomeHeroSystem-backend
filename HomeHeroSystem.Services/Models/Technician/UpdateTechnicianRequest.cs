using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class UpdateTechnicianRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; }
        public int? ExperienceYears { get; set; }
        public int? AddressId { get; set; }
        public bool? IsActive { get; set; } // Thay đổi status
        public List<TechnicianSkillRequest>? Skills { get; set; }
    }
}
