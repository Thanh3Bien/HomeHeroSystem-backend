using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }
        public override async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            try
            {
                return await _dbSet
                    .Where(u => u.IsActive == true && u.IsDeleted == false)
                    .Include(u => u.Address)
                    .OrderBy(u => u.FullName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all active users");
                throw;
            }
        }

        // Get user by email
        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            try
            {
                return await _dbSet
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching user by email: {email}");
                throw;
            }
        }

        // Get user by phone
        public async Task<AppUser?> GetByPhoneAsync(string phone)
        {
            try
            {
                return await _dbSet
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Phone == phone && u.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching user by phone: {phone}");
                throw;
            }
        }
        // Get active users with pagination
        public async Task<(IEnumerable<AppUser> Users, int TotalCount)> GetActiveUsersAsync(int page, int pageSize)
        {
            try
            {
                var query = _dbSet
                    .Where(u => u.IsActive == true && u.IsDeleted == false)
                    .Include(u => u.Address);

                var totalCount = await query.CountAsync();
                var users = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(u => u.FullName)
                    .ToListAsync();

                return (users, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching paginated active users");
                throw;
            }
        }

        // Search users by name or email
        public async Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                return await _dbSet
                    .Where(u => u.IsActive == true && u.IsDeleted == false &&
                               (u.FullName.Contains(searchTerm) || u.Email.Contains(searchTerm)))
                    .Include(u => u.Address)
                    .OrderBy(u => u.FullName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching users with term: {searchTerm}");
                throw;
            }
        }

        public async Task<AppUser?> GetUserByNameAndPhoneAsync(string name, string phone)
        {
            var user = await _context.AppUsers
        .FirstOrDefaultAsync(u => u.FullName == name && u.Phone == phone);
            return user;
        }
    }
}
