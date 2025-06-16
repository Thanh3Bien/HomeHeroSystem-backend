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

        // Address Information
        public string Address { get; set; } = null!;

        // Additional Information
        public string? Note { get; set; }
    }
}
