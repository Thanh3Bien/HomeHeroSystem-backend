using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class TechnicianStatisticsResponse
    {
        public int ActiveTechnicians { get; set; }
        public int InactiveTechnicians { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalJobsCompleted { get; set; }
    }
}
