using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<Service?> GetServiceByNameAsync(string serviceName);
        Task<IEnumerable<string>> GetServiceNamesAsync();
        Task<IEnumerable<string>> SearchServiceNamesAsync(string keyword);


        Task<IEnumerable<Service>> GetActiveByCategoryIdAsync(int categoryId);
        Task<decimal> GetServicePriceAsync(int serviceId);
    }
}
