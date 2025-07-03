using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class GetActiveBookingResponse
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string ServiceType { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PreferredDate { get; set; }
        public string PreferredTime { get; set; }
        public string UrgencyLevel { get; set; }
        public string EstimatedPrice { get; set; }
        public string? ActualPrice { get; set; }
        public TechnicianInfo? TechnicianInfo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<StatusHistoryItem> StatusHistory { get; set; } = new List<StatusHistoryItem>();
    }
}
