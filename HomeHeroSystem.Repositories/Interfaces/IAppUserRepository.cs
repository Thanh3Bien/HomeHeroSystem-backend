using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        Task<(IEnumerable<AppUser> Users, int TotalCount)> GetActiveUsersAsync(int page, int pageSize);

        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByPhoneAsync(string phone);
        Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm);

        //CheckUser
        Task<AppUser?> GetUserByNameAndPhoneAsync(string name, string phone);
    }
}
