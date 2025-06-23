using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<List<string>> GetDistinctDistrictsAsync();
        Task<List<string>> GetWardsByDistrictAsync(string district);
        Task<Address?> FindAddressAsync(string street, string ward, string district, string city);
    }
}
