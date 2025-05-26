using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Services.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AppUserService> _logger;
        public AppUserService(IUnitOfWork unitOfWork, ILogger<AppUserService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                return await _unitOfWork.AppUsers.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users");
                throw;
            }
        }
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching user by id: {id}");
                return await _unitOfWork.AppUsers.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching user by id: {id}");
                throw;
            }
        }
        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation($"Fetching user by email: {email}");
                return await _unitOfWork.AppUsers.GetByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching user by email: {email}");
                throw;
            }
        }

        public async Task<AppUser?> GetUserByPhoneAsync(string phone)
        {
            try
            {
                _logger.LogInformation($"Fetching user by phone: {phone}");
                return await _unitOfWork.AppUsers.GetByPhoneAsync(phone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching user by phone: {phone}");
                throw;
            }
        }
        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            try
            {
                _logger.LogInformation($"Creating new user: {user.Email}");

                // Check if email already exists
                var existingUserByEmail = await _unitOfWork.AppUsers.GetByEmailAsync(user.Email);
                if (existingUserByEmail != null)
                {
                    throw new InvalidOperationException("User with this email already exists");
                }

                // Check if phone already exists
                var existingUserByPhone = await _unitOfWork.AppUsers.GetByPhoneAsync(user.Phone);
                if (existingUserByPhone != null)
                {
                    throw new InvalidOperationException("User with this phone already exists");
                }

                user.CreatedAt = DateTime.Now;
                user.IsActive = true;
                user.IsDeleted = false;
                user.EmailConfirmed = false;

                var createdUser = _unitOfWork.AppUsers.AddEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"User created successfully with id: {createdUser.UserId}");
                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating user: {user.Email}");
                throw;
            }
        }

        public async Task<AppUser?> UpdateUserAsync(int id, AppUser user)
        {
            try
            {
                _logger.LogInformation($"Updating user with id: {id}");

                var existingUser = await _unitOfWork.AppUsers.GetByIdAsync(id);
                if (existingUser == null)
                {
                    _logger.LogWarning($"User with id {id} not found");
                    return null;
                }

                // Update properties
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.AddressId = user.AddressId;
                existingUser.UpdatedAt = DateTime.Now;

                _unitOfWork.AppUsers.UpdateEntity(existingUser);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"User updated successfully with id: {id}");
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating user with id: {id}");
                throw;
            }
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deactivating user with id: {id}");
                var result = await _unitOfWork.AppUsers.DeactivateAsync(id);
                if (result)
                {
                    await _unitOfWork.CompleteAsync();
                    _logger.LogInformation($"User deactivated successfully with id: {id}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deactivating user with id: {id}");
                throw;
            }
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Activating user with id: {id}");

                var user = await _unitOfWork.AppUsers.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with id {id} not found");
                    return false;
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.Now;

                _unitOfWork.AppUsers.UpdateEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"User activated successfully with id: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while activating user with id: {id}");
                throw;
            }
        }
        public async Task<(IEnumerable<AppUser> Users, int TotalCount)> GetActiveUsersAsync(int page, int pageSize)
        {
            try
            {
                _logger.LogInformation($"Fetching active users - Page: {page}, PageSize: {pageSize}");
                return await _unitOfWork.AppUsers.GetActiveUsersAsync(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching paginated active users");
                throw;
            }
        }

        public async Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation($"Searching users with term: {searchTerm}");
                return await _unitOfWork.AppUsers.SearchUsersAsync(searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching users with term: {searchTerm}");
                throw;
            }
        }

    }
}
