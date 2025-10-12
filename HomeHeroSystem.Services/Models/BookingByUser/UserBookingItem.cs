using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.BookingByUser
{
    public class UserBookingItem
    {
        public int BookingId { get; set; }
        public string BookingCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string? PreferredTimeSlot { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? ProblemDescription { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
        public double? TechnicianRating { get; set; }
        public decimal? TotalPrice { get; set; }
        public string FormattedPrice { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        public string? UrgencyLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool CanCancel { get; set; }
        public bool CanRate { get; set; }
        public bool CanReorder { get; set; }
        public bool HasPayment { get; set; }
    }
}
