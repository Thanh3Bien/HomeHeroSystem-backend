﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Booking
{
    public class StatusHistoryItem
    {
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Note { get; set; }

    }
}
