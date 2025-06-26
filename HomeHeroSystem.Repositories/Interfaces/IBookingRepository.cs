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


        Task<IEnumerable<Technician>> GetAvailableTechniciansAsync(int serviceId, DateTime bookingDate, string timeSlot);
        Task<IEnumerable<Technician>> GetTechniciansByLocationAsync(string ward, string district, int serviceId);
        Task<int> CreateAddressAsync(string street, string ward, string district, string city);
        Task<int?> FindExistingAddressAsync(string street, string ward, string district, string city);
        Task<bool> IsTechnicianAvailableAtTimeAsync(int technicianId, DateTime bookingDate, string timeSlot);
        Task<Technician> GetTechnicianWithAddressAsync(int technicianId);


        //GetBookingByTechnician
        Task<(List<Booking> bookings, int totalCount)> GetBookingsByTechnicianIdAsync(
        int technicianId, string? status = null, int page = 1, int pageSize = 10);
        Task<List<string>> GetBookingStatusesByTechnicianAsync(int technicianId);


    }
}
