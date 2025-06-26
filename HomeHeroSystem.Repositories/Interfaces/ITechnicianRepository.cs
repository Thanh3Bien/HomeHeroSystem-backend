using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface ITechnicianRepository : IGenericRepository<Technician>
    {
        //Get method
        Task<Technician?> GetTechnicianByNameAsync(string technicianName);
        Task<(List<Technician> technicians, int totalCount)> GetTechniciansWithFilterAsync(
            string? status,
            string? skill,
            string? search,
            int page,
            int pageSize);
        Task<List<string>> GetTechnicianSkillsAsync(int technicianId);
        Task<decimal> GetTechnicianRatingAsync(int technicianId);
        Task<int> GetTechnicianJobsCountAsync(int technicianId);


        //Set method
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsPhoneExistAsync(string phone);

        Task<bool> IsAddressExistAsync(int addressId);

        Task<bool> IsEmailExistForUpdateAsync(string email, int technicianId);
        Task<bool> IsPhoneExistForUpdateAsync(string phone, int technicianId);


        Task<int> GetActiveTechniciansCountAsync();
        Task<int> GetInactiveTechniciansCountAsync();
        Task<decimal> GetAverageRatingAsync();
        Task<int> GetTotalJobsCompletedAsync();


        Task<IEnumerable<string>> GetTechnicianNamesAsync();
        Task<IEnumerable<string>> SearchTechnicianNamesAsync(string keyword);


        Task<Technician?> GetByEmailAsync(string email);

    }
}
