using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Booking;

namespace HomeHeroSystem.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateBookingRequest> CreateBookingAsync(CreateBookingRequest createBookingRequest)
        {
            try
            {
                var technician = await _unitOfWork.Technicians.GetTechnicianByNameAsync(createBookingRequest.TechnicianName);
                if (technician == null)
                {
                    throw new Exception($"Technician '{createBookingRequest.TechnicianName}' not found");
                }
                var service = await _unitOfWork.Services.GetServiceByNameAsync(createBookingRequest.ServiceName);
                if (service == null)
                {
                    throw new Exception($"Service '{createBookingRequest.ServiceName}' not found");
                }
                int userId;
                var user = await _unitOfWork.AppUsers.GetUserByNameAndPhoneAsync(createBookingRequest.CustomerName, createBookingRequest.Phone);
                if (user == null)
                {
                    userId = await _unitOfWork.Bookings.CreateUserAsync(createBookingRequest.CustomerName,createBookingRequest.Phone);
                }
                else
                {
                    userId = user.UserId;
                }
                var booking = new Booking
                {
                    UserId = userId,
                    TechnicianId = technician.TechnicianId,
                    ServiceId = service.ServiceId,
                    BookingDate = createBookingRequest.BookingDate,
                    Status = createBookingRequest.Status,
                    AddressId = createBookingRequest.AddressId,
                    Note = createBookingRequest.Note,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                };
                _unitOfWork.Bookings.AddEntity(booking);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<CreateBookingRequest>(booking);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateBookingRequest> UpdateBookingAsync(UpdateBookingRequest updateBookingRequest)
        {
            try
            {
                var existingBooking = await _unitOfWork.Bookings.GetBookingByIdAsync(updateBookingRequest.BookingId);
                if (existingBooking != null)
                {
                    var technician = await _unitOfWork.Technicians.GetTechnicianByNameAsync(updateBookingRequest.TechnicianName);
                    if (technician == null)
                    {
                        throw new Exception($"Technician '{updateBookingRequest.TechnicianName}' not found");
                    }
                    var service = await _unitOfWork.Services.GetServiceByNameAsync(updateBookingRequest.ServiceName);
                    if (service == null)
                    {
                        throw new Exception($"Service '{updateBookingRequest.ServiceName}' not found");
                    }
                    int userId;
                    var user = await _unitOfWork.AppUsers.GetUserByNameAndPhoneAsync(updateBookingRequest.CustomerName, updateBookingRequest.Phone);
                    if (user == null)
                    {
                        userId = await _unitOfWork.Bookings.CreateUserAsync(updateBookingRequest.CustomerName, updateBookingRequest.Phone);
                    }
                    else
                    {
                        userId = user.UserId;
                    }
                    existingBooking.UserId = userId;
                    existingBooking.TechnicianId = technician.TechnicianId;
                    existingBooking.ServiceId = service.ServiceId;
                    existingBooking.BookingDate = updateBookingRequest.BookingDate;
                    existingBooking.Status = updateBookingRequest.Status;
                    existingBooking.AddressId = updateBookingRequest.AddressId;
                    existingBooking.Note = updateBookingRequest.Note;
                    existingBooking.IsDeleted = false;
                    existingBooking.UpdatedAt = DateTime.Now;
                    _unitOfWork.Bookings.UpdateEntity(existingBooking);
                    await _unitOfWork.CompleteAsync();
                    
                }
                return _mapper.Map<UpdateBookingRequest>(updateBookingRequest);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteBookingAsync(int id)
        {
            try
            {
                var existingBooking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
                if (existingBooking != null)
                {
                    existingBooking.IsDeleted = true;
                    _unitOfWork.Bookings.UpdateEntity(existingBooking);
                    await _unitOfWork.CompleteAsync();  
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetBookingByIdResponse>> GetAllBookingAsync()
        {
            var listBooking = await _unitOfWork.Bookings.GetAllBookingWithDetailAsync();
            return _mapper.Map<List<GetBookingByIdResponse>>(listBooking);      
        }

        public async Task<List<GetBookingByIdResponse>> GetBookingByStatusAsync(string status)
        {
            var listBooking = await _unitOfWork.Bookings.GetBookingWithStatusAsync(status);
            return _mapper.Map<List<GetBookingByIdResponse>>(listBooking);
        }

        public async Task<GetBookingByIdResponse> GetByIdAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if(booking == null)
            {
                return null;
            }
            return _mapper.Map<GetBookingByIdResponse>(booking); 
        }

        public async Task<List<GetBookingByIdResponse>> SearchBookingAsync(string searchTerm)
        {
            var booking = await _unitOfWork.Bookings.SearchBookingAsync(searchTerm);
            if(booking == null)
            {
                return null;
            }
            return _mapper.Map<List<GetBookingByIdResponse>>(booking);

        }

        public async Task UpdateBookingStatusAsync(int id, string status)
        {
            var existingBooking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
            if (existingBooking != null)
            {
                existingBooking.Status = status;
                _unitOfWork.Bookings.UpdateEntity(existingBooking);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<List<BookingStatusStatistic>> GetBookingStatisticsAsync()
        {
            try
            {
                var allStatuses = await _unitOfWork.Bookings.GetAllBookingStatusesAsync();
                var statistics = new List<BookingStatusStatistic>();
                foreach (var status in allStatuses)
                {
                    var count = await _unitOfWork.Bookings.GetBookingCountByStatusAsync(status);
                    statistics.Add(new BookingStatusStatistic
                    {
                        Status = status,
                        Count = count
                    });
                }
                return statistics.OrderBy(s => s.Status).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
