using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.BookingByTechnician
{
    public class TechnicianBookingItem
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public DateTime BookingDate { get; set; }
        public string? PreferredTimeSlot { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string ServiceName { get; set; }
        public string ProblemDescription { get; set; }
        public string UrgencyLevel { get; set; }
        public decimal? TotalPrice { get; set; }
        public string FormattedPrice { get; set; }
        public string FullAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsUrgent => UrgencyLevel == "urgent";
    }
}
