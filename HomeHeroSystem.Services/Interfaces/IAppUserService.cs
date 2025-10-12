using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Models.AppUser;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<IEnumerable<UserManagementDto>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<AppUser?> GetUserByPhoneAsync(string phone);
        Task<AppUser> CreateUserAsync(CreateUserRequest user);
        Task<AppUser?> UpdateUserAsync(int id, AppUser user);
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> ActivateUserAsync(int id);
        Task<(IEnumerable<AppUser> Users, int TotalCount)> GetActiveUsersAsync(int page, int pageSize);
        Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm);

        Task<GetPersonalInformationResponse?> GetPersonalInformationAsync(int userId);
        Task<GetPersonalInformationResponse?> UpdatePersonalInformationAsync(int userId, UpdatePersonalInformationRequest request);
        Task<ChangePasswordResponse> ChangePasswordAsync(int userId, ChangePasswordRequest request);

    }
}
