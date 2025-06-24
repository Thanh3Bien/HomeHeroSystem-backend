using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class TechnicianAssignmentResult
    {
        public int? TechnicianId { get; set; }
        public string AssignmentType { get; set; }
        public string Message { get; set; }
    }
}
