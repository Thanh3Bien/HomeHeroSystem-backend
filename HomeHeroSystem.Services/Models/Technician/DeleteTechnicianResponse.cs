using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class DeleteTechnicianResponse
    {
        public int TechnicianId { get; set; }
        public string Message { get; set; } = "Technician deleted successfully";
    }
}
