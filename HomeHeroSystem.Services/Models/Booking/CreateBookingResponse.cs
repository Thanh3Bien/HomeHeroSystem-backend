using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class CreateBookingResponse
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string FormattedPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; }
    }
}
