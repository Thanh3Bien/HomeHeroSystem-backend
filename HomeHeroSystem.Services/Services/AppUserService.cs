using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.AppUser;
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
        public async Task<IEnumerable<UserManagementDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users for management");

                // Lấy raw data từ repository
                var users = await _unitOfWork.AppUsers.GetAllAsync();

                // Transform sang DTO
                var userDtos = new List<UserManagementDto>();

                foreach (var user in users)
                {
                    var userDto = new UserManagementDto
                    {
                        UserId = user.UserId,
                        FullName = user.FullName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Address = BuildFullAddress(user.Address),
                        Status = (bool)user.IsActive ? "Active" : "Inactive",
                        JoinDate = (DateTime)user.CreatedAt,
                        Avatar = DetermineAvatar(user.FullName),
                        EmailConfirmed = (bool)user.EmailConfirmed,
                        LastLoginDate = user.LastLoginDate
                    };

                    userDtos.Add(userDto);
                }

                return userDtos.OrderBy(u => u.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users");
                throw;
            }
        }
        private string DetermineAvatar(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "👤";

            // Simple logic để determine avatar dựa trên first character
            var firstChar = fullName.Trim().ToLower()[0];
            return firstChar switch
            {
                >= 'a' and <= 'm' => "👨",
                >= 'n' and <= 'z' => "👩",
                _ => "👤"
            };
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
        public async Task<AppUser> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation($"Creating new user: {request.Email}");

                var existingUserByEmail = await _unitOfWork.AppUsers.GetByEmailAsync(request.Email);
                if (existingUserByEmail != null)
                {
                    throw new InvalidOperationException("User with this email already exists");
                }

                var existingUserByPhone = await _unitOfWork.AppUsers.GetByPhoneAsync(request.Phone);
                if (existingUserByPhone != null)
                {
                    throw new InvalidOperationException("User with this phone already exists");
                }
                var user = new AppUser
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    PasswordHash = request.PasswordHash,


                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    EmailConfirmed = false,
                    AddressId = null, 
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null,
                    LastLoginDate = null,
                    UpdatedAt = null
                };

                var createdUser = _unitOfWork.AppUsers.AddEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"User created successfully with id: {createdUser.UserId}");
                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating user: {request.Email}");
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
        public async Task<GetPersonalInformationResponse?> GetPersonalInformationAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"Fetching personal information for user id: {userId}");

                // Get user with address
                var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
                if (user == null || user.IsDeleted == true)
                {
                    _logger.LogWarning($"User with id {userId} not found or deleted");
                    return null;
                }

                // Get user statistics
                var statistics = await GetUserStatisticsAsync(userId);

                // Build address info
                AddressInfo? addressInfo = null;
                if (user.Address != null)
                {
                    addressInfo = new AddressInfo
                    {
                        AddressId = user.Address.AddressId,
                        Street = user.Address.Street,
                        Ward = user.Address.Ward,
                        District = user.Address.District,
                        City = user.Address.City,
                        FullAddress = BuildFullAddress(user.Address)
                    };
                }

                // Build response
                var response = new GetPersonalInformationResponse
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    LastLoginDate = user.LastLoginDate,
                    Address = addressInfo,
                    Statistics = statistics,
                    CanChangePassword = true
                };

                _logger.LogInformation($"Personal information retrieved successfully for user id: {userId}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching personal information for user id: {userId}");
                throw;
            }
        }

        public async Task<GetPersonalInformationResponse?> UpdatePersonalInformationAsync(int userId, UpdatePersonalInformationRequest request)
        {
            try
            {
                _logger.LogInformation($"Updating personal information for user id: {userId}");

                // Get existing user
                var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
                if (user == null || user.IsDeleted == true)
                {
                    _logger.LogWarning($"User with id {userId} not found or deleted");
                    return null;
                }

                // Check email uniqueness (if changed)
                if (user.Email != request.Email)
                {
                    var existingUserByEmail = await _unitOfWork.AppUsers.GetByEmailAsync(request.Email);
                    if (existingUserByEmail != null && existingUserByEmail.UserId != userId)
                    {
                        throw new InvalidOperationException("Email already exists for another user");
                    }
                }

                // Check phone uniqueness (if changed)
                if (user.Phone != request.Phone)
                {
                    var existingUserByPhone = await _unitOfWork.AppUsers.GetByPhoneAsync(request.Phone);
                    if (existingUserByPhone != null && existingUserByPhone.UserId != userId)
                    {
                        throw new InvalidOperationException("Phone number already exists for another user");
                    }
                }

                // Update user information
                user.FullName = request.FullName;
                user.Email = request.Email;
                user.Phone = request.Phone;
                user.UpdatedAt = DateTime.Now;

                // Handle address update
                if (request.Address != null)
                {
                    if (user.AddressId.HasValue && user.Address != null)
                    {
                        // Update existing address
                        user.Address.Street = request.Address.Street;
                        user.Address.Ward = request.Address.Ward;
                        user.Address.District = request.Address.District;
                        user.Address.City = request.Address.City;
                        user.Address.UpdatedAt = DateTime.Now;
                        _unitOfWork.Addresses.UpdateEntity(user.Address);
                    }
                    else
                    {
                        // Create new address
                        var newAddress = new HomeHeroSystem.Repositories.Entities.Address
                        {
                            Street = request.Address.Street,
                            Ward = request.Address.Ward,
                            District = request.Address.District,
                            City = request.Address.City,
                            CreatedAt = DateTime.Now,
                            IsDeleted = false
                        };
                        _unitOfWork.Addresses.AddEntity(newAddress);
                        await _unitOfWork.CompleteAsync(); // Save to get AddressId

                        user.AddressId = newAddress.AddressId;
                    }
                }

                // Save changes
                _unitOfWork.AppUsers.UpdateEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"Personal information updated successfully for user id: {userId}");

                // Return updated information
                return await GetPersonalInformationAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating personal information for user id: {userId}");
                throw;
            }
        }
        public async Task<ChangePasswordResponse> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            try
            {
                _logger.LogInformation($"Changing password for user id: {userId}");

                // Get user
                var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
                if (user == null || user.IsDeleted == true)
                {
                    return new ChangePasswordResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Verify current password (simple string comparison)
                if (request.CurrentPassword != user.PasswordHash)
                {
                    return new ChangePasswordResponse
                    {
                        Success = false,
                        Message = "Mật khẩu hiện tại không đúng"
                    };
                }

                // Update password (no hashing, just plain text)
                user.PasswordHash = request.NewPassword;
                user.UpdatedAt = DateTime.Now;

                _unitOfWork.AppUsers.UpdateEntity(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"Password changed successfully for user id: {userId}");

                return new ChangePasswordResponse
                {
                    Success = true,
                    Message = "Đổi mật khẩu thành công",
                    ChangedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while changing password for user id: {userId}");
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi đổi mật khẩu"
                };
            }
        }

        private async Task<UserStatistics> GetUserStatisticsAsync(int userId)
        {
            try
            {
                var (allBookings, totalCount) = await _unitOfWork.Bookings.GetBookingsByUserIdAsync(
                    userId, status: null, page: 1, pageSize: int.MaxValue);

                var totalBookings = totalCount;
                var completedBookings = allBookings.Count(b => b.Status?.ToLower() == "completed");
                var cancelledBookings = allBookings.Count(b => b.Status?.ToLower() == "cancelled");
                var pendingBookings = allBookings.Count(b => b.Status?.ToLower() == "pending" ||
                                                          b.Status?.ToLower() == "confirmed" ||
                                                          b.Status?.ToLower() == "inprogress" ||
                                                          b.Status?.ToLower() == "waitingtechnician");

                var totalSpent = allBookings.Where(b => b.Status?.ToLower() == "completed" && b.TotalPrice.HasValue)
                                        .Sum(b => b.TotalPrice.Value);

                var lastBookingDate = allBookings.OrderByDescending(b => b.CreatedAt).FirstOrDefault()?.CreatedAt;

                // Calculate average rating (from feedbacks)
                var completedBookingIds = allBookings.Where(b => b.Status?.ToLower() == "completed")
                                                 .Select(b => b.BookingId).ToList();

                double? averageRating = null;
                if (completedBookingIds.Any())
                {
                    var feedbacks = await _unitOfWork.Feedbacks.GetFeedbacksByBookingIdsAsync(completedBookingIds);
                    if (feedbacks.Any())
                    {
                        averageRating = feedbacks.Average(f => f.Rating ?? 0);
                    }
                }

                return new UserStatistics
                {
                    TotalBookings = totalBookings,
                    CompletedBookings = completedBookings,
                    CancelledBookings = cancelledBookings,
                    PendingBookings = pendingBookings,
                    TotalSpent = totalSpent,
                    FormattedTotalSpent = $"{totalSpent:N0} ₫",
                    AverageRating = averageRating,
                    LastBookingDate = lastBookingDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calculating statistics for user {userId}");
                return new UserStatistics();
            }
        }


        private string BuildFullAddress(HomeHeroSystem.Repositories.Entities.Address address)
        {
            if (address == null) return string.Empty;

            var addressParts = new List<string>();
            if (!string.IsNullOrEmpty(address.Street)) addressParts.Add(address.Street);
            if (!string.IsNullOrEmpty(address.Ward)) addressParts.Add(address.Ward);
            if (!string.IsNullOrEmpty(address.District)) addressParts.Add(address.District);
            if (!string.IsNullOrEmpty(address.City)) addressParts.Add(address.City);

            return addressParts.Any() ? string.Join(", ", addressParts) : string.Empty;
        }

    }
}
