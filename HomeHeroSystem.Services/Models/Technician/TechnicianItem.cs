using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class TechnicianItem
    {
        public int TechnicianId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public List<string> Skills { get; set; } = new List<string>();
        public decimal Rating { get; set; }
        public int JobsCount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
