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
    }
}
