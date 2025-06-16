using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Models.Booking;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<GetBookingByIdResponse> GetByIdAsync(int id);
        Task<List<GetBookingByIdResponse>> SearchBookingAsync(string searchTerm);

        Task<List<GetBookingByIdResponse>> GetAllBookingAsync();    
        Task<List<GetBookingByIdResponse>> GetBookingByStatusAsync(string status);

        Task<List<BookingStatusStatistic>> GetBookingStatisticsAsync();

        Task<CreateBookingRequest> CreateBookingAsync(CreateBookingRequest createBookingRequest);   

        Task<UpdateBookingRequest> UpdateBookingAsync(UpdateBookingRequest updateBookingRequest);

        Task UpdateBookingStatusAsync(int id, string status);

        Task DeleteBookingAsync(int id);

        
    }
}
