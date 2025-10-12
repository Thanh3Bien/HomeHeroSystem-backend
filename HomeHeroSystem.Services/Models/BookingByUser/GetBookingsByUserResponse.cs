using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.BookingByUser
{
    public class GetBookingsByUserResponse
    {
        public List<UserBookingItem> Orders { get; set; } = new List<UserBookingItem>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string? FilteredStatus { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
