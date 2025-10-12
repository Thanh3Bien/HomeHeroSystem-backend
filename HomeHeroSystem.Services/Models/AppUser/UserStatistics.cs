using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.AppUser
{
    public class UserStatistics
    {
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int PendingBookings { get; set; }
        public decimal TotalSpent { get; set; }
        public string FormattedTotalSpent { get; set; } = "0 ₫";
        public double? AverageRating { get; set; }
        public DateTime? LastBookingDate { get; set; }
    }
}
