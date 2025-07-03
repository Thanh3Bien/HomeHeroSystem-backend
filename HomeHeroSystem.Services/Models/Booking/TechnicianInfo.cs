using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class TechnicianInfo
    {
        public int TechnicianId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Rating { get; set; }
        public string Experience { get; set; }
        public string Avatar { get; set; } = null;
    }
}
