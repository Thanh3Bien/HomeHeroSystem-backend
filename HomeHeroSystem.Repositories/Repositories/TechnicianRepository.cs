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
    public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
    {
        public TechnicianRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public async Task<Technician?> GetTechnicianByNameAsync(string technicianName)
        {
            var technician = await _context.Technicians.FirstOrDefaultAsync(t => t.FullName == technicianName && t.IsDeleted != true);
            if (technician == null)
            {
                return null;
            }
            return technician;
        }


        public async Task<(List<Technician> technicians, int totalCount)> GetTechniciansWithFilterAsync(string? status, string? skill, string? search, int page, int pageSize)
        {
            try
            {
                var query = _context.Technicians.Where(t => t.IsDeleted != true);
                if(!string.IsNullOrEmpty(status) && status.ToLower() != "all")
                {
                    bool isActive = status.ToLower() == "active";
                    query = query.Where(t => t.IsActive != isActive);
                }
                if (!string.IsNullOrEmpty(skill))
                {
                    query = query.Where(t => _context.TechnicianSkills
                        .Where(ts => ts.TechnicianId == t.TechnicianId)
                        .Join(_context.Skills, ts => ts.SkillId, s => s.SkillId, (ts, s) => s.SkillName)
                        .Any(skillName => skillName.ToLower().Contains(skill.ToLower())));
                }
                if (!string.IsNullOrEmpty(search))
                {
                    var searchLower = search.ToLower();
                    query = query.Where(t =>
                        t.FullName.ToLower().Contains(searchLower) ||
                        t.Email.ToLower().Contains(searchLower) ||
                        t.Phone.Contains(search) ||
                        _context.TechnicianSkills
                            .Where(ts => ts.TechnicianId == t.TechnicianId)
                            .Join(_context.Skills, ts => ts.SkillId, s => s.SkillId, (ts, s) => s.SkillName)
                            .Any(skillName => skillName.ToLower().Contains(searchLower))
                            );
                }
                var totalCount = await query.CountAsync();
                var technicians = await query
                    .OrderBy(t => t.FullName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return (technicians, totalCount);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetTechnicianJobsCountAsync(int technicianId)
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.TechnicianId == technicianId && b.Status == "Completed")
                    .CountAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> GetTechnicianRatingAsync(int technicianId)
        {
            try
            {
                var ratings = await _context.Feedbacks
                    .Where(f => f.TechnicianId == technicianId && f.Rating.HasValue)
                    .Select(f => f.Rating.Value)
                    .ToListAsync();
                return ratings.Any() ? (decimal)ratings.Average() : 0;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<string>> GetTechnicianSkillsAsync(int technicianId)
        {
            try
            {
                return await _context.TechnicianSkills
                    .Where(ts => ts.TechnicianId == technicianId)
                    .Join(_context.Skills, ts => ts.SkillId, s => s.SkillId, (ts, s) => s.SkillName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> IsEmailExistAsync(string email)
        {
            try
            {
                return await _context.Technicians
                    .AnyAsync(t => t.Email.ToLower() == email.ToLower() && t.IsDeleted != true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        
        public async Task<bool> IsPhoneExistAsync(string phone)
        {
            try
            {
                return await _context.Technicians
                    .AnyAsync(t => t.Phone == phone && t.IsDeleted != true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);   
            }
        }
        //need edit
        public async Task<bool> IsAddressExistAsync(int AddressId)
        {
            try
            {
                return await _context.Addresses
                    .AnyAsync(a => a.AddressId == AddressId && a.IsDeleted != true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsEmailExistForUpdateAsync(string email, int technicianId)
        {
            try
            {
                return await _dbSet
                    .AnyAsync(t => t.Email.ToLower() == email.ToLower()
                                  && t.TechnicianId != technicianId
                                  && t.IsDeleted != true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email existence for update");
                throw;
            }
        }

        public async Task<bool> IsPhoneExistForUpdateAsync(string phone, int technicianId)
        {
            try
            {
                return await _dbSet
                    .AnyAsync(t => t.Phone == phone
                                  && t.TechnicianId != technicianId
                                  && t.IsDeleted != true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking phone existence for update");
                throw;
            }
        }


        public async Task<int> GetActiveTechniciansCountAsync()
        {
            return await _dbSet
                .CountAsync(t => t.IsActive == true && t.IsDeleted != true);
        }

        public async Task<int> GetInactiveTechniciansCountAsync()
        {
            return await _dbSet
                .CountAsync(t => t.IsActive == false && t.IsDeleted != true);
        }

        public async Task<decimal> GetAverageRatingAsync()
        {
            var ratings = await _context.Feedbacks
                .Where(r => r.TechnicianId != null)
                .Select(r => r.Rating)
                .ToListAsync();

            return ratings.Any() ? (decimal)ratings.Average() : 0;
        }

        public async Task<int> GetTotalJobsCompletedAsync()
        {
            return await _context.Bookings
                .CountAsync(b => b.Status == "completed");
        }

        public async Task<IEnumerable<string>> GetTechnicianNamesAsync()
        {
            return await _context.Technicians
                .Where(t => t.IsActive == true && !string.IsNullOrEmpty(t.FullName))
                .Select(t => t.FullName)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> SearchTechnicianNamesAsync(string keyword)
        {
            return await _context.Technicians
                .Where(t => t.IsActive == true &&
                           !string.IsNullOrEmpty(t.FullName) &&
                           t.FullName.Contains(keyword))
                .Select(t => t.FullName)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();
        }

        public async Task<Technician?> GetByEmailAsync(string email)
        {
            try
            {
                return await _dbSet
                    .Include(t => t.Address)
                    .Include(t => t.TechnicianSkills)
                        .ThenInclude(ts => ts.Skill)
                    .FirstOrDefaultAsync(t => t.Email == email && t.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching technician by email: {email}");
                throw;
            }
        }

    }
}
