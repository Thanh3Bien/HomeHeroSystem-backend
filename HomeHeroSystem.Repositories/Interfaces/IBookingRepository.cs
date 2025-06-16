using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        //get method
        Task<Booking?> GetBookingByIdAsync(int id);

        Task<List<Booking>> SearchBookingAsync(string searchTerm);
        Task<List<Booking>> GetAllBookingWithDetailAsync();
        Task<List<Booking>> GetBookingWithStatusAsync(string status);

        //set method
        Task<Booking> CreateAsync(Booking newBooking);

        Task<int> CreateUserAsync(string name, string phone);

        Task<int> GetBookingCountByStatusAsync(string status);

        Task<List<string>> GetAllBookingStatusesAsync();


    }
}
