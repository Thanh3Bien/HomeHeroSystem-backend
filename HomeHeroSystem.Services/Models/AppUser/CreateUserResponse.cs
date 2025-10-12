using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.AppUser
{
    public class CreateUserResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool? IsActive { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
