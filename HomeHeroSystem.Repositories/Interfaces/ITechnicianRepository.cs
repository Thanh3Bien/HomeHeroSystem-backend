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
        Task<Technician?> GetTechnicianByNameAsync(string technicianName); 
    }
}
