using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;
using static HomeHeroSystem.Services.Models.Dashboard.GetDashboard;

namespace HomeHeroSystem.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IUnitOfWork unitOfWork, ILogger<DashboardService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<DashboardStatisticsResponse> GetDashboardStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching dashboard statistics");

                // Get all data
                var bookingStats = await GetBookingStatisticsAsync();
                var technicianStats = await GetTechnicianStatisticsAsync();
                var userStats = await GetUserStatisticsAsync();
                var revenueStats = await GetRevenueStatisticsAsync();
                var monthlyRevenue = await GetMonthlyRevenueAsync();
                var servicePerformance = await GetServicePerformanceAsync();

                return new DashboardStatisticsResponse
                {
                    Bookings = bookingStats,
                    Technicians = technicianStats,
                    Users = userStats,
                    Revenue = revenueStats,
                    MonthlyRevenue = monthlyRevenue,
                    ServicePerformance = servicePerformance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching dashboard statistics");
                throw;
            }
        }

        private async Task<BookingStatistics> GetBookingStatisticsAsync()
        {
            try
            {
                var allBookings = await _unitOfWork.Bookings.GetAllAsync();

                return new BookingStatistics
                {
                    Total = allBookings.Count(),
                    Completed = allBookings.Count(b => b.Status?.ToLower() == "completed"),
                    Pending = allBookings.Count(b => b.Status?.ToLower() == "pending"),
                    Confirmed = allBookings.Count(b => b.Status?.ToLower() == "confirmed"),
                    InProgress = allBookings.Count(b => b.Status?.ToLower() == "inprogress"),
                    Cancelled = allBookings.Count(b => b.Status?.ToLower() == "cancelled")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking statistics");
                return new BookingStatistics();
            }
        }

        private async Task<TechnicianStatistics> GetTechnicianStatisticsAsync()
        {
            try
            {
                var allTechnicians = await _unitOfWork.Technicians.GetAllAsync();
                var totalJobsCompleted = await _unitOfWork.Technicians.GetTotalJobsCompletedAsync();
                var avgRating = await _unitOfWork.Technicians.GetAverageRatingAsync();
                

                return new TechnicianStatistics
                {
                    Total = allTechnicians.Count(),
                    Active = allTechnicians.Count(t => t.IsActive == true),
                    Inactive = allTechnicians.Count(t => t.IsActive != true),
                    AvgRating = (double)avgRating,
                    TotalJobs = totalJobsCompleted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting technician statistics");
                return new TechnicianStatistics();
            }
        }

        private async Task<UserStatistics> GetUserStatisticsAsync()
        {
            try
            {
                var allUsers = await _unitOfWork.AppUsers.GetAllAsync();

                // Calculate VIP customers (users with high spending)
                var vipCustomers = 0;
                var totalRating = 0.0;
                var ratingCount = 0;

                foreach (var user in allUsers)
                {
                    // Get user bookings to calculate spending
                    var (userBookings, _) = await _unitOfWork.Bookings.GetBookingsByUserIdAsync(
                        user.UserId, null, 1, int.MaxValue);

                    var totalSpent = userBookings.Where(b => b.Status?.ToLower() == "completed" && b.TotalPrice.HasValue)
                        .Sum(b => b.TotalPrice.Value);

                    // VIP if spent over 5M VND or has 15+ bookings
                    if (totalSpent >= 5000000 || userBookings.Count() >= 15)
                    {
                        vipCustomers++;
                    }

                    // Calculate average rating from user's feedbacks
                    var completedBookingIds = userBookings.Where(b => b.Status?.ToLower() == "completed")
                        .Select(b => b.BookingId).ToList();

                    if (completedBookingIds.Any())
                    {
                        var feedbacks = await _unitOfWork.Feedbacks.GetFeedbacksByBookingIdsAsync(completedBookingIds);
                        if (feedbacks.Any())
                        {
                            totalRating += feedbacks.Average(f => f.Rating ?? 0);
                            ratingCount++;
                        }
                    }
                }

                return new UserStatistics
                {
                    Total = allUsers.Count(),
                    Active = allUsers.Count(u => u.IsActive == true),
                    Inactive = allUsers.Count(u => u.IsActive != true),
                    VipCustomers = vipCustomers,
                    AvgRating = ratingCount > 0 ? Math.Round(totalRating / ratingCount, 1) : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user statistics");
                return new UserStatistics();
            }
        }

        private async Task<RevenueStatistics> GetRevenueStatisticsAsync()
        {
            try
            {
                var allBookings = await _unitOfWork.Bookings.GetAllAsync();
                var completedBookings = allBookings.Where(b => b.Status?.ToLower() == "completed");

                var today = DateTime.Today;
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                var startOfLastMonth = startOfMonth.AddMonths(-1);
                var yesterday = today.AddDays(-1);

                // Today's revenue
                var todayRevenue = completedBookings
                    .Where(b => b.BookingDate.Date == today && b.TotalPrice.HasValue)
                    .Sum(b => b.TotalPrice.Value);

                // Yesterday's revenue for growth calculation
                var yesterdayRevenue = completedBookings
                    .Where(b => b.BookingDate.Date == yesterday && b.TotalPrice.HasValue)
                    .Sum(b => b.TotalPrice.Value);

                // This month's revenue
                var thisMonthRevenue = completedBookings
                    .Where(b => b.BookingDate >= startOfMonth && b.TotalPrice.HasValue)
                    .Sum(b => b.TotalPrice.Value);

                // Last month's revenue for growth calculation
                var lastMonthRevenue = completedBookings
                    .Where(b => b.BookingDate >= startOfLastMonth && b.BookingDate < startOfMonth && b.TotalPrice.HasValue)
                    .Sum(b => b.TotalPrice.Value);

                // Total revenue
                var totalRevenue = completedBookings
                    .Where(b => b.TotalPrice.HasValue)
                    .Sum(b => b.TotalPrice.Value);

                // Average order value
                var avgOrderValue = completedBookings.Any() ?
                    completedBookings.Where(b => b.TotalPrice.HasValue).Average(b => b.TotalPrice.Value) : 0;

                // Growth calculations
                var todayGrowth = yesterdayRevenue > 0 ?
                    ((double)(todayRevenue - yesterdayRevenue) / (double)yesterdayRevenue) * 100 : 0;

                var monthGrowth = lastMonthRevenue > 0 ?
                    ((double)(thisMonthRevenue - lastMonthRevenue) / (double)lastMonthRevenue) * 100 : 0;

                return new RevenueStatistics
                {
                    Today = todayRevenue,
                    TodayFormatted = FormatCurrency(todayRevenue),
                    ThisMonth = thisMonthRevenue,
                    ThisMonthFormatted = FormatCurrency(thisMonthRevenue),
                    TotalRevenue = totalRevenue,
                    TotalRevenueFormatted = FormatCurrency(totalRevenue),
                    AvgOrderValue = avgOrderValue,
                    AvgOrderValueFormatted = FormatCurrency(avgOrderValue),
                    TodayGrowthPercentage = Math.Round(todayGrowth, 1),
                    MonthGrowthPercentage = Math.Round(monthGrowth, 1)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting revenue statistics");
                return new RevenueStatistics();
            }
        }

        private async Task<List<MonthlyRevenueData>> GetMonthlyRevenueAsync()
        {
            try
            {
                var allBookings = await _unitOfWork.Bookings.GetAllAsync();
                var completedBookings = allBookings.Where(b => b.Status?.ToLower() == "completed");

                var monthlyData = new List<MonthlyRevenueData>();
                var currentDate = DateTime.Now;

                for (int i = 11; i >= 0; i--)
                {
                    var month = currentDate.AddMonths(-i);
                    var startOfMonth = new DateTime(month.Year, month.Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    var monthBookings = completedBookings
                        .Where(b => b.BookingDate >= startOfMonth && b.BookingDate <= endOfMonth);

                    var revenue = monthBookings.Where(b => b.TotalPrice.HasValue).Sum(b => b.TotalPrice.Value);
                    var bookingCount = monthBookings.Count();

                    // Calculate growth vs previous month
                    var prevMonth = startOfMonth.AddMonths(-1);
                    var prevMonthEnd = startOfMonth.AddDays(-1);
                    var prevMonthRevenue = completedBookings
                        .Where(b => b.BookingDate >= prevMonth && b.BookingDate <= prevMonthEnd && b.TotalPrice.HasValue)
                        .Sum(b => b.TotalPrice.Value);

                    var growthPercentage = prevMonthRevenue > 0 ?
                        ((double)(revenue - prevMonthRevenue) / (double)prevMonthRevenue) * 100 : 0;

                    monthlyData.Add(new MonthlyRevenueData
                    {
                        Month = month.ToString("MMM yyyy"),
                        Revenue = revenue,
                        Bookings = bookingCount,
                        Growth = growthPercentage >= 0 ? $"+{growthPercentage:F0}%" : $"{growthPercentage:F0}%",
                        GrowthPercentage = Math.Round(growthPercentage, 1)
                    });
                }

                return monthlyData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly revenue data");
                return new List<MonthlyRevenueData>();
            }
        }

        private async Task<List<ServicePerformanceData>> GetServicePerformanceAsync()
        {
            try
            {
                var allBookings = await _unitOfWork.Bookings.GetAllAsync();
                var completedBookings = allBookings.Where(b => b.Status?.ToLower() == "completed");

                var serviceGroups = completedBookings
                    .Where(b => !string.IsNullOrEmpty(b.Service.ServiceName))
                    .GroupBy(b => b.Service.ServiceName)
                    .Select(g => new
                    {
                        ServiceName = g.Key,
                        Revenue = g.Where(b => b.TotalPrice.HasValue).Sum(b => b.TotalPrice.Value),
                        BookingCount = g.Count()
                    })
                    .OrderByDescending(s => s.Revenue)
                    .Take(5)
                    .ToList();

                var totalRevenue = serviceGroups.Sum(s => s.Revenue);

                return serviceGroups.Select(s => new ServicePerformanceData
                {
                    Name = s.ServiceName,
                    Revenue = s.Revenue,
                    RevenueFormatted = FormatCurrency(s.Revenue),
                    Percentage = totalRevenue > 0 ? Math.Round((double)(s.Revenue / totalRevenue) * 100, 1) : 0,
                    BookingCount = s.BookingCount
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting service performance data");
                return new List<ServicePerformanceData>();
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return $"{amount:N0} VND";
        }
    }
}
