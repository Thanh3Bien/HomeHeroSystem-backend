using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.ServiceCategory
{
    public class ServiceCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
        public string BasePrice { get; set; } 
        public int ServiceCount { get; set; }
    }
}
