using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class BookingStatisticResponse
    {
        public List<BookingStatusStatistic> StatusStatistics { get; set; } = new List<BookingStatusStatistic>();
    }
}
