using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IServiceCategoryRepository : IGenericRepository<ServiceCategory>
    {
        Task<IEnumerable<ServiceCategory>> GetActiveAsync();
    }
}
