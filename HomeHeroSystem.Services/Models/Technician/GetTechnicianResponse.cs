using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Technician
{
    public class GetTechnicianResponse
    {
        public List<TechnicianItem> Technicians { get; set; } = new List<TechnicianItem>();
        public int TotalCount { get; set; }
        public int Page {  get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; } 

    }
}
