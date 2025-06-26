using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.BookingByTechnician
{
    public class GetBookingsByTechnicianRequest
    {
        public int TechnicianId { get; set; }
        public string? Status { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
