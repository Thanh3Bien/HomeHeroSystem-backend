using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.BookingByTechnician
{
    public class GetBookingsByTechnicianResponse
    {
        public List<TechnicianBookingItem> Bookings { get; set; } = new List<TechnicianBookingItem>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string? FilteredStatus { get; set; }
        public string Message { get; set; }
    }
}
