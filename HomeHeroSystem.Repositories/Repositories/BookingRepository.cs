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
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(HomeHeroContext context, ILogger logger)
           : base(context, logger)
        {
        }

        public async Task<Booking> CreateAsync(Booking newBooking)
        {
            try
            {
                await _dbSet.AddAsync(newBooking);
                return newBooking;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CreateUserAsync(string name, string phone)
        {
            var user = new AppUser
            {
                FullName = name,
                Phone = phone,
                Email = $"{phone}@temp.com",
                PasswordHash = "1",
                CreatedAt = DateTime.Now
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        

        public async Task<List<Booking>> GetAllBookingWithDetailAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.Technician)
                .Include(b => b.Address)
                .Where(b => b.IsDeleted != true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.Technician)
                .Include(b => b.Address)
                .Include(b => b.Payments)
                .Include(b => b.Feedbacks)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.IsDeleted != true);
        }

        


        public async Task<List<Booking>> GetBookingWithStatusAsync(string status)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.Technician)
                .Include(b => b.Address)
                .Where(b => b.Status == status && b.IsDeleted != true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> SearchBookingAsync(string searchTerm)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.Technician)
                .Include(b => b.Address)
                .Where(b => b.IsDeleted != true &&
                    (b.User.FullName.Contains(searchTerm) ||
                     b.User.Phone.Contains(searchTerm) ||
                     b.Service.ServiceName.Contains(searchTerm) ||
                     b.Technician.FullName.Contains(searchTerm) ||
                     b.BookingId.ToString().Contains(searchTerm)))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return booking;
        }


        public async Task<List<string>> GetAllBookingStatusesAsync()
        {
            return await _context.Bookings
            .Where(b => b.IsDeleted != true)
            .Select(b => b.Status)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();
        }


        public async Task<int> GetBookingCountByStatusAsync(string status)
        {
            return await _context.Bookings
            .Where(b => b.IsDeleted != true && b.Status == status)
            .CountAsync();
        }


        public async Task<IEnumerable<Technician>> GetAvailableTechniciansAsync(int serviceId, DateTime bookingDate, string timeSlot)
        {
            var dateOnly = bookingDate.Date;

            // Get technicians who have skills for this service and are not booked at this time
            var availableTechnicians = await _context.Technicians
                .Include(t => t.Address)
                .Where(t => t.IsActive == true && t.IsDeleted != true &&
                           !_context.Bookings.Any(b =>
                               b.TechnicianId == t.TechnicianId &&
                               b.BookingDate.Date == dateOnly &&
                               b.PreferredTimeSlot == timeSlot &&
                               b.Status != "Cancelled" &&
                               b.IsDeleted == false))
                .ToListAsync();

            return availableTechnicians;
        }

        public async Task<IEnumerable<Technician>> GetTechniciansByLocationAsync(string ward, string district, int serviceId)
        {
            var allAvailableTechnicians = await GetAvailableTechniciansAsync(serviceId, DateTime.Now, "");

            return allAvailableTechnicians.Where(t => t.Address != null &&
                (t.Address.Ward == ward || t.Address.District == district));
        }

        public async Task<int> CreateAddressAsync(string street, string ward, string district, string city)
        {
            var address = new Address
            {
                Street = street,
                Ward = ward,
                District = district,
                City = city,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address.AddressId;
        }

        public async Task<int?> FindExistingAddressAsync(string street, string ward, string district, string city)
        {
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a =>
                    a.Street == street &&
                    a.Ward == ward &&
                    a.District == district &&
                    a.City == city &&
                    a.IsDeleted == false);

            return existingAddress?.AddressId;
        }

        public async Task<bool> IsTechnicianAvailableAtTimeAsync(int technicianId, DateTime bookingDate, string timeSlot)
        {
            var dateOnly = bookingDate.Date;

            var conflictingBookings = await _context.Bookings
                .CountAsync(b =>
                    b.TechnicianId == technicianId &&
                    b.BookingDate.Date == dateOnly &&
                    b.PreferredTimeSlot == timeSlot &&
                    b.Status != "Cancelled" &&
                    b.IsDeleted == false);

            return conflictingBookings == 0;
        }

        public async Task<Technician> GetTechnicianWithAddressAsync(int technicianId)
        {
            return await _context.Technicians
                .Include(t => t.Address)
                .FirstOrDefaultAsync(t => t.TechnicianId == technicianId && t.IsActive == true && t.IsDeleted == false);
        }

        public async Task<(List<Booking> bookings, int totalCount)> GetBookingsByTechnicianIdAsync(
    int technicianId, string? status = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Bookings
            .Include(b => b.Service)
            .Include(b => b.Address)
            .Include(b => b.Technician)
            .Include(b => b.User)
            .Where(b => b.TechnicianId == technicianId && b.IsDeleted != true);
                if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
                {
                    query = query.Where(b => b.Status.ToLower() == status.ToLower());
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply sorting (newest first) and pagination
                var bookings = await query
                    .OrderByDescending(b => b.BookingDate)
                    .ThenByDescending(b => b.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (bookings, totalCount);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<string>> GetBookingStatusesByTechnicianAsync(int technicianId)
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.TechnicianId == technicianId && b.IsDeleted != true)
                    .Select(b => b.Status)
                    .Distinct()
                    .OrderBy(s => s)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking statuses for technician ID: {TechnicianId}", technicianId);
                throw new Exception($"Error retrieving booking statuses: {ex.Message}");
            }
        }

        public async Task<Booking?> GetActiveBookingByUserIdAsync(int userId)
        {
            try
            {

                var activeBooking = await _context.Bookings
                    .Include(b => b.Service)
                    .Include(b => b.Technician)
                    .Include(b => b.Address)
                    .Where(b => b.UserId == userId &&
                               b.IsDeleted != true &&
                               b.Status != "Completed" &&
                               b.Status != "Cancelled")
                    .OrderByDescending(b => b.CreatedAt)
                    .FirstOrDefaultAsync();

                return activeBooking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active booking for user ID: {UserId}", userId);
                throw new Exception($"Error retrieving active booking: {ex.Message}");
            }
        }

        public async Task<List<Booking>> GetUnpaidBookingsByUserIdAsync(int userId)
        {
            try
            {
                var unpaidBookings = await _context.Bookings
                    .Include(b => b.Service)
                    .Include(b => b.Technician)
                    .Include(b => b.Address)
                    .Where(b => b.UserId == userId &&
                               b.Status == "Completed" &&
                               b.IsDeleted != true &&
                               !_context.Payments.Any(p => p.BookingId == b.BookingId))
                    .OrderByDescending(b => b.UpdatedAt ?? b.CreatedAt)
                    .ToListAsync();

                return unpaidBookings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unpaid bookings for user ID: {UserId}", userId);
                throw new Exception($"Error retrieving unpaid bookings: {ex.Message}");
            }
        }
    }
}
