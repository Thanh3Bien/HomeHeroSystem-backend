using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class CreateBookingRequest
    {
        public string CustomerName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string TechnicianName { get; set; } = null!;

        public string ServiceName { get; set; } = null!;

        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = null!;

        public int AddressId { get; set; }

        public string? Note { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

    }
}
