using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class GetBookingByIdResponse
    {
        public int BookingId { get; set; }

        // Customer Information
        public string CustomerName { get; set; } = null!;
        public string Phone { get; set; } = null!;

        // Service Information
        public string ServiceName { get; set; } = null!;
        public decimal Price { get; set; }

        // Technician Information
        public string TechnicianName { get; set; } = null!;

        // Booking Information
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = null!;

        public string? PreferredTimeSlot { get; set; }
        public string? ProblemDescription { get; set; }
        public string? UrgencyLevel { get; set; }
        public decimal? UrgencyFee { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? CustomerPhone { get; set; }
        public string? Note { get; set; }
        // Address Information
        public string Address { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string District { get; set; } = null!;
        public string City { get; set; } = null!;

       
    }
}
