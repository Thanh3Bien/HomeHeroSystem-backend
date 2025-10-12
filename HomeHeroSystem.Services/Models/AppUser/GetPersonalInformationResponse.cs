using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.AppUser
{
    public class GetPersonalInformationResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public AddressInfo? Address { get; set; }
        public UserStatistics Statistics { get; set; } = new UserStatistics();
        public bool CanChangePassword { get; set; } = true;
    }

    public class UserManagementDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; } // "Active" or "Inactive"
        public DateTime JoinDate { get; set; }
        public string Avatar { get; set; } = "👤";


        // Additional Info
        public bool EmailConfirmed { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class UserManagementResponse
    {
        public List<UserManagementDto> Users { get; set; } = new List<UserManagementDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public UserManagementStatistics Statistics { get; set; }
    }

    public class UserManagementStatistics
    {
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalBookings { get; set; }
        public double AverageRating { get; set; }
        public int VipCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class BookingHistoryDto
    {
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string TechnicianName { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public string FormattedPrice { get; set; }
        public int? Rating { get; set; }
        public string Feedback { get; set; }
    }

    public class FeedbackHistoryDto
    {
        public int BookingId { get; set; }
        public string ServiceName { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime Date { get; set; }
        public string TechnicianName { get; set; }
    }

    public class UserDetailDto : UserManagementDto
    {
        public List<BookingHistoryDto> BookingHistory { get; set; } = new List<BookingHistoryDto>();
        public List<FeedbackHistoryDto> FeedbackHistory { get; set; } = new List<FeedbackHistoryDto>();
        public AddressInfo Address { get; set; }
    }
}
