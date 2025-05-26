using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace HomeHeroSystem.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly int _accessTokenExpireMinutes;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            ILogger<AuthenticationService> logger,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _logger = logger;
            _accessTokenExpireMinutes = int.Parse(configuration["JWT:AccessTokenExpireMinutes"] ?? "60");
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", loginRequest.Email);

                // Get user by email
                var user = await _unitOfWork.AppUsers.GetByEmailAsync(loginRequest.Email);

                // Validate user exists and is active
                if (user == null || user.IsActive != true || user.IsDeleted == true)
                {
                    _logger.LogWarning("User not found or inactive: {Email}", loginRequest.Email);
                    return null;
                }

                // ✅ SIMPLE STRING COMPARISON - So sánh trực tiếp password
                bool isPasswordValid = string.Equals(user.PasswordHash, loginRequest.Password, StringComparison.Ordinal);

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Invalid password for user: {Email}. Expected: {Expected}, Got: {Got}",
                        loginRequest.Email, user.PasswordHash, loginRequest.Password);
                    return null;
                }

                // Generate access token
                var accessToken = _jwtService.GenerateAccessToken(user);

                // Update last login date
                user.LastLoginDate = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                ((IGenericRepository<AppUser>)_unitOfWork.AppUsers).UpdateEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("User logged in successfully: {Email}", user.Email);

                return new LoginResponse
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    AccessToken = accessToken,
                    TokenExpiry = DateTime.UtcNow.AddMinutes(_accessTokenExpireMinutes),
                    UserType = "AppUser"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for email: {Email}", loginRequest.Email);
                throw;
            }
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Logout attempt for user: {UserId}", userId);

                var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for logout: {UserId}", userId);
                    return false;
                }

                // Update last activity
                user.UpdatedAt = DateTime.UtcNow;

                ((IGenericRepository<AppUser>)_unitOfWork.AppUsers).UpdateEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("User logged out successfully: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout for user: {UserId}", userId);
                throw;
            }
        }

    }
}
