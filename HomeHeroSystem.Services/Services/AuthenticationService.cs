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

        public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email} as {UserType}",
                    loginRequest.Email, loginRequest.UserType);

                if (loginRequest.UserType.ToLower() == "user")
                {
                    return await LoginUserAsync(loginRequest);
                }
                else if (loginRequest.UserType.ToLower() == "technician")
                {
                    return await LoginTechnicianAsync(loginRequest);
                }
                else
                {
                    _logger.LogWarning("Invalid user type: {UserType}", loginRequest.UserType);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for email: {Email}", loginRequest.Email);
                throw;
            }
        }

        private async Task<LoginResponse?> LoginUserAsync(LoginRequest loginRequest)
        {
            var user = await _unitOfWork.AppUsers.GetByEmailAsync(loginRequest.Email);

            if (user == null || user.IsActive != true || user.IsDeleted == true)
            {
                _logger.LogWarning("User not found or inactive: {Email}", loginRequest.Email);
                return null;
            }

            // Simple string comparison for password
            bool isPasswordValid = string.Equals(user.PasswordHash, loginRequest.Password, StringComparison.Ordinal);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for user: {Email}", loginRequest.Email);
                return null;
            }

            // Generate access token
            var accessToken = _jwtService.GenerateAccessToken(user);

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.AppUsers.UpdateEntity(user);
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
                UserType = "User",
                AddressId = user.AddressId
            };
        }

        private async Task<LoginResponse?> LoginTechnicianAsync(LoginRequest loginRequest)
        {
            var technician = await _unitOfWork.Technicians.GetByEmailAsync(loginRequest.Email);

            if (technician == null || technician.IsActive != true || technician.IsDeleted == true)
            {
                _logger.LogWarning("Technician not found or inactive: {Email}", loginRequest.Email);
                return null;
            }

            // Simple string comparison for password
            bool isPasswordValid = string.Equals(technician.PasswordHash, loginRequest.Password, StringComparison.Ordinal);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for technician: {Email}", loginRequest.Email);
                return null;
            }

            var accessToken = _jwtService.GenerateAccessToken(technician);

            // Update last login date
            technician.LastLoginDate = DateTime.UtcNow;
            technician.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Technicians.UpdateEntity(technician);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Technician logged in successfully: {Email}", technician.Email);

            return new LoginResponse
            {
                UserId = technician.TechnicianId,
                FullName = technician.FullName,
                Email = technician.Email,
                Phone = technician.Phone,
                AccessToken = accessToken,
                TokenExpiry = DateTime.UtcNow.AddMinutes(_accessTokenExpireMinutes),
                UserType = "Technician",
                ExperienceYears = technician.ExperienceYears,
                AddressId = technician.AddressId
            };
        }

        public async Task<bool> LogoutAsync(int userId, string userType)
        {
            try
            {
                _logger.LogInformation("Logout attempt for {UserType} ID: {UserId}", userType, userId);

                if (userType.ToLower() == "user")
                {
                    var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
                    if (user == null)
                    {
                        _logger.LogWarning("User not found for logout: {UserId}", userId);
                        return false;
                    }

                    user.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.AppUsers.UpdateEntity(user);
                }
                else if (userType.ToLower() == "technician")
                {
                    var technician = await _unitOfWork.Technicians.GetByIdAsync(userId);
                    if (technician == null)
                    {
                        _logger.LogWarning("Technician not found for logout: {UserId}", userId);
                        return false;
                    }

                    technician.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.Technicians.UpdateEntity(technician);
                }
                else
                {
                    return false;
                }

                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("{UserType} logged out successfully: {UserId}", userType, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout for {UserType}: {UserId}", userType, userId);
                throw;
            }
        }

        public async Task<CurrentUserResponse?> GetCurrentUserAsync(int userId, string userType)
        {
            try
            {
                if (userType.ToLower() == "user")
                {
                    var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
                    if (user == null || user.IsDeleted == true)
                        return null;

                    return new CurrentUserResponse
                    {
                        UserId = user.UserId,
                        FullName = user.FullName,
                        Email = user.Email,
                        Phone = user.Phone,
                        UserType = "User",
                        AddressId = user.AddressId,
                        IsActive = user.IsActive ?? false,
                        LastLoginDate = user.LastLoginDate
                    };
                }
                else if (userType.ToLower() == "technician")
                {
                    var technician = await _unitOfWork.Technicians.GetByIdAsync(userId);
                    if (technician == null || technician.IsDeleted == true)
                        return null;

                    return new CurrentUserResponse
                    {
                        UserId = technician.TechnicianId,
                        FullName = technician.FullName,
                        Email = technician.Email,
                        Phone = technician.Phone,
                        UserType = "Technician",
                        ExperienceYears = technician.ExperienceYears,
                        Skills = technician.TechnicianSkills?.Select(ts => ts.Skill?.SkillName ?? "").ToList(),
                        AddressId = technician.AddressId,
                        IsActive = technician.IsActive ?? false,
                        LastLoginDate = technician.LastLoginDate
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting current {UserType}: {UserId}", userType, userId);
                throw;
            }
        }

    }
}
