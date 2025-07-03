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
using HomeHeroSystem.Services.Models.BookingByTechnician;
using HomeHeroSystem.Services.Models.Technician;

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

        public async Task<CreateBookingResponse> CreateBookingWithAutoAssignAsync(CreateBookingWithAutoAssignRequest request)
        {
            try
            {
                // 1. Get or create address
                var addressId = await _unitOfWork.Bookings.FindExistingAddressAsync(
                    request.Street, request.Ward, request.District, request.City);

                if (!addressId.HasValue)
                {
                    addressId = await _unitOfWork.Bookings.CreateAddressAsync(
                        request.Street, request.Ward, request.District, request.City);
                }

                // 2. Get or create user
                int userId;
                if (request.UserId.HasValue)
                {
                    userId = request.UserId.Value;
                }
                else
                {
                    userId = await _unitOfWork.Bookings.CreateUserAsync(
                        request.CustomerName, request.CustomerPhone);
                }

                // 3. Get service and calculate price
                var service = await _unitOfWork.Services.GetByIdAsync(request.ServiceId);
                if (service == null)
                {
                    throw new Exception($"Service with ID {request.ServiceId} not found");
                }

                var urgencyFee = request.UrgencyLevel == "urgent" ? 50000 : 0;
                var totalPrice = service.Price + urgencyFee;

                // 4. Find best available technician
                var technicianAssignment = await FindBestTechnicianAsync(
                    request.ServiceId, request.BookingDate, request.PreferredTimeSlot,
                    request.Ward, request.District);

                // 5. Determine booking status
                string bookingStatus = technicianAssignment.TechnicianId.HasValue ? "Confirmed" : "WaitingTechnician";

                // 6. Create booking
                var booking = new Booking
                {
                    UserId = userId,
                    TechnicianId = (int)technicianAssignment.TechnicianId,
                    ServiceId = request.ServiceId,
                    BookingDate = request.BookingDate,
                    PreferredTimeSlot = request.PreferredTimeSlot,
                    Status = bookingStatus,
                    AddressId = addressId.Value,
                    ProblemDescription = request.ProblemDescription,
                    UrgencyLevel = request.UrgencyLevel,
                    UrgencyFee = urgencyFee,
                    TotalPrice = totalPrice,
                    CustomerName = request.CustomerName,
                    CustomerPhone = request.CustomerPhone,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                _unitOfWork.Bookings.AddEntity(booking);
                await _unitOfWork.CompleteAsync();

                // 7. Get assigned technician info
                AssignedTechnicianInfo assignedTechnicianInfo = null;
                if (technicianAssignment.TechnicianId.HasValue)
                {
                    var technician = await _unitOfWork.Bookings.GetTechnicianWithAddressAsync(
                        technicianAssignment.TechnicianId.Value);

                    assignedTechnicianInfo = new AssignedTechnicianInfo
                    {
                        TechnicianId = technician.TechnicianId,
                        FullName = technician.FullName,
                        Phone = technician.Phone,
                        AssignmentMessage = technicianAssignment.Message,
                        IsAutoAssigned = true
                    };
                }
                else
                {
                    assignedTechnicianInfo = new AssignedTechnicianInfo
                    {
                        TechnicianId = null,
                        FullName = "Chưa phân công",
                        Phone = "",
                        AssignmentMessage = "Đang tìm thợ phù hợp, chúng tôi sẽ liên hệ sớm nhất.",
                        IsAutoAssigned = false
                    };
                }

                // 8. Generate booking code
                var bookingCode = $"BK{booking.BookingId:D8}";

                // 9. Return response
                return new CreateBookingResponse
                {
                    BookingId = booking.BookingId,
                    Status = booking.Status,
                    TotalPrice = totalPrice,
                    FormattedPrice = $"{totalPrice:N0} ₫",
                    CreatedAt = booking.CreatedAt.Value,
                    Message = technicianAssignment.TechnicianId.HasValue
                        ? "Đặt lịch thành công! Thợ đã được phân công và sẽ liên hệ với bạn trong 30 phút."
                        : "Đặt lịch thành công! Chúng tôi đang tìm thợ phù hợp và sẽ liên hệ với bạn trong 1 giờ.",
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo booking: {ex.Message}");
            }
        }

        public async Task<TechnicianAssignmentResult> FindBestTechnicianAsync(
            int serviceId, DateTime bookingDate, string timeSlot, string ward, string district)
        {
            try
            {
                // 1. Try to find technician in same ward first
                var sameWardTechnicians = await _unitOfWork.Bookings.GetTechniciansByLocationAsync(ward, district, serviceId);
                var sameWardAvailable = sameWardTechnicians.Where(t =>
                    t.Address.Ward == ward &&
                    _unitOfWork.Bookings.IsTechnicianAvailableAtTimeAsync(t.TechnicianId, bookingDate, timeSlot).Result);

                if (sameWardAvailable.Any())
                {
                    var bestTechnician = sameWardAvailable
                        .OrderByDescending(t => t.ExperienceYears)
                        .First();

                    return new TechnicianAssignmentResult
                    {
                        TechnicianId = bestTechnician.TechnicianId,
                        AssignmentType = "SameWard",
                        Message = $"Đã phân công thợ {bestTechnician.FullName} (cùng phường)"
                    };
                }

                // 2. Try to find technician in same district
                var sameDistrictAvailable = sameWardTechnicians.Where(t =>
                    t.Address.District == district &&
                    _unitOfWork.Bookings.IsTechnicianAvailableAtTimeAsync(t.TechnicianId, bookingDate, timeSlot).Result);

                if (sameDistrictAvailable.Any())
                {
                    var bestTechnician = sameDistrictAvailable
                        .OrderByDescending(t => t.ExperienceYears)
                        .First();

                    return new TechnicianAssignmentResult
                    {
                        TechnicianId = bestTechnician.TechnicianId,
                        AssignmentType = "SameDistrict",
                        Message = $"Đã phân công thợ {bestTechnician.FullName} (cùng quận)"
                    };
                }

                // 3. Find any available technician with required skills
                var availableTechnicians = await _unitOfWork.Bookings.GetAvailableTechniciansAsync(
                    serviceId, bookingDate, timeSlot);

                if (availableTechnicians.Any())
                {
                    var bestTechnician = availableTechnicians
                        .OrderByDescending(t => t.ExperienceYears)
                        .First();

                    return new TechnicianAssignmentResult
                    {
                        TechnicianId = bestTechnician.TechnicianId,
                        AssignmentType = "Available",
                        Message = $"Đã phân công thợ {bestTechnician.FullName} (khu vực lân cận)"
                    };
                }

                // 4. No available technician found
                return new TechnicianAssignmentResult
                {
                    TechnicianId = null,
                    AssignmentType = "None",
                    Message = "Hiện tại chưa có thợ trống, chúng tôi sẽ sắp xếp và liên hệ sớm nhất"
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm thợ: {ex.Message}");
            }
        }

        public async Task<GetBookingsByTechnicianResponse> GetBookingsByTechnicianIdAsync(GetBookingsByTechnicianRequest request)
        {
            try
            {
                // 1. Validate technician exists
                var technician = await _unitOfWork.Technicians.GetByIdAsync(request.TechnicianId);
                if (technician == null || technician.IsDeleted == true)
                {
                    throw new ArgumentException("Technician not found");
                }

                // 2. Get bookings with pagination and filtering
                var (bookings, totalCount) = await _unitOfWork.Bookings.GetBookingsByTechnicianIdAsync(
                    request.TechnicianId, request.Status, request.Page, request.PageSize);

                // 3. Map to response items
                var bookingItems = new List<TechnicianBookingItem>();
                foreach (var booking in bookings)
                {
                    var bookingItem = new TechnicianBookingItem
                    {
                        BookingId = booking.BookingId,
                        Status = booking.Status,
                        BookingDate = booking.BookingDate,
                        PreferredTimeSlot = booking.PreferredTimeSlot,
                        CustomerName = booking.CustomerName ?? booking.User?.FullName ?? "N/A",
                        CustomerPhone = booking.CustomerPhone ?? booking.User?.Phone ?? "N/A",
                        ServiceName = booking.Service?.ServiceName ?? "N/A",
                        ProblemDescription = booking.ProblemDescription ?? "N/A",
                        UrgencyLevel = booking.UrgencyLevel ?? "normal",
                        TotalPrice = booking.TotalPrice,
                        FormattedPrice = booking.TotalPrice.HasValue ? $"{booking.TotalPrice:N0} ₫" : "N/A",
                        FullAddress = BuildFullAddress(booking.Address),
                        CreatedAt = booking.CreatedAt ?? DateTime.Now,
                        CompletedAt = booking.Status?.ToLower() == "completed" ? booking.UpdatedAt : null
                    };

                    bookingItems.Add(bookingItem);
                }

                // 4. Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                // 5. Build response
                return new GetBookingsByTechnicianResponse
                {
                    Bookings = bookingItems,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    FilteredStatus = request.Status,
                    Message = $"Retrieved {bookingItems.Count} bookings for technician {technician.FullName}"
                };
            }
            catch (ArgumentException)
            {
                throw; // Re-throw ArgumentException for controller to handle
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving technician bookings: {ex.Message}");
            }
        }

        private string BuildFullAddress(Address address)
        {
            if (address == null) return "N/A";

            var addressParts = new List<string>();

            if (!string.IsNullOrEmpty(address.Street)) addressParts.Add(address.Street);
            if (!string.IsNullOrEmpty(address.Ward)) addressParts.Add(address.Ward);
            if (!string.IsNullOrEmpty(address.District)) addressParts.Add(address.District);
            if (!string.IsNullOrEmpty(address.City)) addressParts.Add(address.City);

            return addressParts.Any() ? string.Join(", ", addressParts) : "N/A";
        }

        // Thêm vào BookingService class
        public async Task<GetActiveBookingResponse?> GetActiveBookingByUserIdAsync(int userId)
        {
            try
            {
                // Get active booking from repository
                var activeBooking = await _unitOfWork.Bookings.GetActiveBookingByUserIdAsync(userId);
                var technician = await _unitOfWork.Technicians.GetByIdAsync(activeBooking.TechnicianId);

                if (activeBooking == null)
                {
                    return null;
                }

                // Build full address
                var fullAddress = BuildFullAddress(activeBooking.Address);
                var rating = await _unitOfWork.Technicians.GetTechnicianRatingAsync(technician.TechnicianId);

                // Build technician info (if assigned)
                TechnicianInfo? technicianInfo = null;
                if (activeBooking.Technician != null)
                {
                    technicianInfo = new TechnicianInfo
                    {
                        TechnicianId = activeBooking.Technician.TechnicianId,
                        Name = activeBooking.Technician.FullName,
                        Phone = activeBooking.Technician.Phone,
                        Rating = rating,
                        Experience = $"{activeBooking.Technician.ExperienceYears ?? 1} năm kinh nghiệm"
                    };
                }

                // Build status history (simplified version)
                var statusHistory = new List<StatusHistoryItem>();

                // Add created status
                if (activeBooking.CreatedAt.HasValue)
                {
                    statusHistory.Add(new StatusHistoryItem
                    {
                        Status = "Pending",
                        Timestamp = activeBooking.CreatedAt.Value,
                        Note = "Đơn hàng đã được tạo"
                    });
                }

                // Add current status if different from Pending
                if (activeBooking.Status != "Pending" && activeBooking.UpdatedAt.HasValue)
                {
                    var statusNote = activeBooking.Status switch
                    {
                        "Confirmed" => "Đã phân công thợ và xác nhận lịch hẹn",
                        "InProgress" => "Thợ đang tiến hành sửa chữa",
                        "WaitingTechnician" => "Đang tìm thợ phù hợp",
                        _ => $"Trạng thái được cập nhật thành {activeBooking.Status}"
                    };

                    statusHistory.Add(new StatusHistoryItem
                    {
                        Status = activeBooking.Status,
                        Timestamp = activeBooking.UpdatedAt.Value,
                        Note = statusNote
                    });
                }

                // Build response
                var response = new GetActiveBookingResponse
                {
                    BookingId = activeBooking.BookingId,
                    Status = activeBooking.Status,
                    CustomerName = activeBooking.CustomerName ?? "N/A",
                    CustomerPhone = activeBooking.CustomerPhone ?? "N/A",
                    ServiceType = activeBooking.Service?.ServiceName ?? "N/A",
                    Description = activeBooking.ProblemDescription ?? "N/A",
                    Address = fullAddress,
                    PreferredDate = activeBooking.BookingDate.ToString("yyyy-MM-dd"),
                    PreferredTime = activeBooking.PreferredTimeSlot ?? "N/A",
                    UrgencyLevel = activeBooking.UrgencyLevel ?? "normal",
                    EstimatedPrice = activeBooking.TotalPrice.HasValue ? $"{activeBooking.TotalPrice:N0}" : "0",
                    ActualPrice = activeBooking.Status == "Completed" && activeBooking.TotalPrice.HasValue ?
                                 $"{activeBooking.TotalPrice:N0}" : null,
                    TechnicianInfo = technicianInfo,
                    CreatedAt = activeBooking.CreatedAt ?? DateTime.Now,
                    UpdatedAt = activeBooking.UpdatedAt ?? activeBooking.CreatedAt ?? DateTime.Now,
                    StatusHistory = statusHistory.OrderBy(s => s.Timestamp).ToList()
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting active booking for user {userId}: {ex.Message}");
            }
        }

        public async Task<GetUnpaidBookingsResponse> GetUnpaidBookingsByUserIdAsync(int userId)
        {
            try
            {
                var unpaidBookings = await _unitOfWork.Bookings.GetUnpaidBookingsByUserIdAsync(userId);

                var unpaidItems = unpaidBookings.Select(b => new UnpaidBookingItem
                {
                    BookingId = b.BookingId,
                    ServiceName = b.Service?.ServiceName ?? "N/A",
                    TechnicianName = b.Technician?.FullName ?? "N/A",
                    CompletedAt = b.UpdatedAt ?? b.CreatedAt ?? DateTime.Now,
                    TotalPrice = b.TotalPrice ?? 0,
                    FormattedPrice = $"{(b.TotalPrice ?? 0):N0} ₫",
                    BookingCode = $"BK{b.BookingId:D8}"
                }).ToList();

                var totalAmount = unpaidItems.Sum(x => x.TotalPrice);

                return new GetUnpaidBookingsResponse
                {
                    UnpaidBookings = unpaidItems,
                    TotalCount = unpaidItems.Count,
                    TotalAmount = totalAmount,
                    FormattedTotalAmount = $"{totalAmount:N0} ₫",
                    Message = unpaidItems.Count > 0
                        ? $"Có {unpaidItems.Count} đơn hàng cần thanh toán"
                        : "Không có đơn hàng nào cần thanh toán"
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting unpaid bookings for user {userId}: {ex.Message}");
            }
        }
    }
}
