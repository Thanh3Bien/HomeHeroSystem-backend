using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<AppUser?> GetUserByPhoneAsync(string phone);
        Task<AppUser> CreateUserAsync(AppUser user);
        Task<AppUser?> UpdateUserAsync(int id, AppUser user);
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> ActivateUserAsync(int id);
        Task<(IEnumerable<AppUser> Users, int TotalCount)> GetActiveUsersAsync(int page, int pageSize);
        Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm);
    }
}
