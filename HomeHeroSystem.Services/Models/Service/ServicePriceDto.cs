using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Service
{
    public class ServicePriceDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal UrgencyFee { get; set; }
        public decimal TotalPrice { get; set; }
        public string FormattedBasePrice { get; set; }
        public string FormattedUrgencyFee { get; set; }
        public string FormattedTotalPrice { get; set; }
        public string UrgencyLevel { get; set; }
    }
}
