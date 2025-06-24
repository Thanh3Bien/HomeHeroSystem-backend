using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class CreateBookingWithAutoAssignRequest
    {
        public int ServiceId { get; set; }
        public DateTime BookingDate { get; set; }
        public string PreferredTimeSlot { get; set; } 
        public string ProblemDescription { get; set; }
        public string UrgencyLevel { get; set; } = "normal"; 

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }

        public int? UserId { get; set; }
    }
}
