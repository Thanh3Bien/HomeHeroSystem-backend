using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class AssignedTechnicianInfo
    {
        public int? TechnicianId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string AssignmentMessage { get; set; }
        public bool IsAutoAssigned { get; set; }
    }
}
