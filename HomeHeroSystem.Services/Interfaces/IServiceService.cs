using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Service;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<string>> GetServiceNamesAsync();
        Task<IEnumerable<string>> SearchServiceNamesAsync(string keyword);

        Task<IEnumerable<ServiceDto>> GetServicesByCategoryAsync(int categoryId);
        Task<ServicePriceDto> GetServicePriceAsync(int serviceId, string urgencyLevel = "normal");
        Task<ServiceDto> GetServiceByIdAsync(int serviceId);
    }
}

