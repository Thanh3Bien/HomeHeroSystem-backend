using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Authentication;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest loginRequest);
        Task<bool> LogoutAsync(int userId, string userType);
        Task<CurrentUserResponse?> GetCurrentUserAsync(int userId, string userType);
    }
}
