using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class UnpaidBookingItem
    {
        public int BookingId { get; set; }
        public string ServiceName { get; set; }
        public string TechnicianName { get; set; }
        public DateTime CompletedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string FormattedPrice { get; set; }
        public string BookingCode { get; set; }
    }
}
