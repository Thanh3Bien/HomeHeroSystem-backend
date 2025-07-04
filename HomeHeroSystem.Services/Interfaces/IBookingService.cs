﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Models.Booking;
using HomeHeroSystem.Services.Models.BookingByTechnician;
using HomeHeroSystem.Services.Models.Technician;

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


        Task<CreateBookingResponse> CreateBookingWithAutoAssignAsync(CreateBookingWithAutoAssignRequest request);
        Task<TechnicianAssignmentResult> FindBestTechnicianAsync(int serviceId, DateTime bookingDate, string timeSlot, string ward, string district);


        //GetBookingByTechnicianId
        Task<GetBookingsByTechnicianResponse> GetBookingsByTechnicianIdAsync(GetBookingsByTechnicianRequest request);



        Task<GetActiveBookingResponse?> GetActiveBookingByUserIdAsync(int userId);
        Task<GetUnpaidBookingsResponse> GetUnpaidBookingsByUserIdAsync(int userId);


    }
}
