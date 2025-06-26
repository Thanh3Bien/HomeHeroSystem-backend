using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Technician;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface ITechnicianService
    {
        //Get Technicians with conditions
        Task<GetTechnicianResponse> GetTechnicianAsync(GetTechnicianRequest request);

        Task<CreateTechnicianResponse> CreateTechnicianAsync(CreateTechnicianRequest request);
        Task<UpdateTechnicianResponse> UpdateTechnicianAsync(int id, UpdateTechnicianRequest request);

        Task<DeleteTechnicianResponse> DeleteTechnicianAsync(int id);

        Task<UpdateStatusResponse> UpdateTechnicianStatusAsync(int id, UpdateStatusRequest request);


        Task<TechnicianStatisticsResponse> GetTechnicianStatisticsAsync();


        Task<IEnumerable<string>> GetTechnicianNamesAsync();
        Task<IEnumerable<string>> SearchTechnicianNamesAsync(string keyword);


        Task<TechnicianItem> GetTechnicianByIdAsync(int technicianId);
    }
}
