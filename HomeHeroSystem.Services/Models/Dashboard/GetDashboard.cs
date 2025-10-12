using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Dashboard
{
    public class GetDashboard
    {
        public class DashboardStatisticsResponse
        {
            public BookingStatistics Bookings { get; set; }
            public TechnicianStatistics Technicians { get; set; }
            public UserStatistics Users { get; set; }
            public RevenueStatistics Revenue { get; set; }
            public List<MonthlyRevenueData> MonthlyRevenue { get; set; }
            public List<ServicePerformanceData> ServicePerformance { get; set; }
        }

        public class BookingStatistics
        {
            public int Total { get; set; }
            public int Completed { get; set; }
            public int Pending { get; set; }
            public int Confirmed { get; set; }
            public int InProgress { get; set; }
            public int Cancelled { get; set; }
        }

        public class TechnicianStatistics
        {
            public int Total { get; set; }
            public int Active { get; set; }
            public int Inactive { get; set; }
            public double AvgRating { get; set; }
            public int TotalJobs { get; set; }
        }

        public class UserStatistics
        {
            public int Total { get; set; }
            public int Active { get; set; }
            public int Inactive { get; set; }
            public int VipCustomers { get; set; }
            public double AvgRating { get; set; }
        }

        public class RevenueStatistics
        {
            public decimal Today { get; set; }
            public string TodayFormatted { get; set; }
            public decimal ThisMonth { get; set; }
            public string ThisMonthFormatted { get; set; }
            public decimal TotalRevenue { get; set; }
            public string TotalRevenueFormatted { get; set; }
            public decimal AvgOrderValue { get; set; }
            public string AvgOrderValueFormatted { get; set; }
            public double TodayGrowthPercentage { get; set; }
            public double MonthGrowthPercentage { get; set; }
        }

        public class MonthlyRevenueData
        {
            public string Month { get; set; }
            public decimal Revenue { get; set; }
            public int Bookings { get; set; }
            public string Growth { get; set; }
            public double GrowthPercentage { get; set; }
        }

        public class ServicePerformanceData
        {
            public string Name { get; set; }
            public decimal Revenue { get; set; }
            public string RevenueFormatted { get; set; }
            public double Percentage { get; set; }
            public int BookingCount { get; set; }
        }
    }
}
