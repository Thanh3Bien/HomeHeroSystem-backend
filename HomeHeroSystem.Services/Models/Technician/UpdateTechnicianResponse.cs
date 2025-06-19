using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class UpdateTechnicianResponse
    {
        public int TechnicianId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Message { get; set; } = "Technician updated successfully";
    }
}
