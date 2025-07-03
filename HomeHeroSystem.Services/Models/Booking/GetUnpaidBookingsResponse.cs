using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class GetUnpaidBookingsResponse
    {
        public List<UnpaidBookingItem> UnpaidBookings { get; set; } = new List<UnpaidBookingItem>();
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount { get; set; }
        public string Message { get; set; }
    }
}
