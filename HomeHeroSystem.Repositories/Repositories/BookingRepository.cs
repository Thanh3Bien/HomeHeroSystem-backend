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
    }
}
