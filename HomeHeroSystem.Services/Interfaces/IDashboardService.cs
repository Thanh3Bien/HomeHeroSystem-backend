using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HomeHeroSystem.Services.Models.Dashboard.GetDashboard;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatisticsResponse> GetDashboardStatisticsAsync();
    }
}
