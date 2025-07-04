﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Authentication
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public DateTime TokenExpiry { get; set; }
        public string UserType { get; set; } = null!; // "User" or "Technician"
        public int? ExperienceYears { get; set; } // Only for Technician
        public List<string>? Skills { get; set; } // Only for Technician
        public int? AddressId { get; set; }
    }
}
