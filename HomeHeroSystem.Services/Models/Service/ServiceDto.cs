using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Service
{
    public class ServiceDto
    {
        public int ServiceId { get; set; }
        public int CategoryId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string FormattedPrice { get; set; } 
        public int EstimatedTime { get; set; }
        public string EstimatedTimeText { get; set; } 
        public bool IsActive { get; set; }
    }
}
