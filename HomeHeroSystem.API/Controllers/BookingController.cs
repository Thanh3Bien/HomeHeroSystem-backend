using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Booking;
using HomeHeroSystem.Services.Models.BookingByTechnician;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllBookingAsync()
        {
            try
            {
                var result = await _bookingService.GetAllBookingAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _bookingService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBookingAsync([FromQuery] string searchTerm)
        {
            try
            {
                var result = await _bookingService.SearchBookingAsync(searchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterBookingByStatusAsync([FromQuery] string status)
        {
            try
            {
                var result = await _bookingService.GetBookingByStatusAsync(status); 
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetBookingStatisticsAsync()
        {
            try
            {
                var result = await _bookingService.GetBookingStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingRequest createBookingRequest)
        {
            try
            {

                var result = await _bookingService.CreateBookingAsync(createBookingRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookingAsync([FromBody] UpdateBookingRequest updateBookingRequest)
        {
            try
            {

                var result = await _bookingService.UpdateBookingAsync(updateBookingRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateBookingStatusAsync(int id, string status)
        {
            try
            {
                await _bookingService.UpdateBookingStatusAsync(id, status);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBookingAsync(int id)
        {
            try
            {
                await _bookingService.DeleteBookingAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        [HttpPost("createform")]
        public async Task<IActionResult> CreateBookingWithAutoAssignRequest([FromBody] CreateBookingWithAutoAssignRequest request)
        {
            try
            {


                var result = await _bookingService.CreateBookingWithAutoAssignAsync(request);

                return Ok(new
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = (object)null
                });
            }
        }

        [HttpGet("available-technicians")]
        public async Task<IActionResult> GetAvailableTechnicians(
            [FromQuery] int serviceId,
            [FromQuery] DateTime date,
            [FromQuery] string timeSlot,
            [FromQuery] string ward,
            [FromQuery] string district)
        {
            try
            {
                var result = await _bookingService.FindBestTechnicianAsync(serviceId, date, timeSlot, ward, district);

                return Ok(new
                {
                    IsSuccess = true,
                    Message = "Tìm thợ thành công",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = (object)null
                });
            }
        }


        [HttpGet("technician/{technicianId}")]
        public async Task<IActionResult> GetBookingsByTechnicianIdAsync(
    int technicianId,
    [FromQuery] string? status = null,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate parameters
                if (technicianId <= 0)
                {
                    return BadRequest(new { message = "Invalid technician ID. ID must be greater than 0." });
                }

                if (page <= 0)
                {
                    return BadRequest(new { message = "Page must be greater than 0." });
                }

                if (pageSize <= 0 || pageSize > 100)
                {
                    return BadRequest(new { message = "Page size must be between 1 and 100." });
                }

                // Build request
                var request = new GetBookingsByTechnicianRequest
                {
                    TechnicianId = technicianId,
                    Status = status,
                    Page = page,
                    PageSize = pageSize
                };

                // Get bookings
                var result = await _bookingService.GetBookingsByTechnicianIdAsync(request);

                return Ok(new
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Data = result,
                    Pagination = new
                    {
                        CurrentPage = result.Page,
                        PageSize = result.PageSize,
                        TotalCount = result.TotalCount,
                        TotalPages = result.TotalPages,
                        HasPrevious = result.Page > 1,
                        HasNext = result.Page < result.TotalPages
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = (object)null
                });
            }
            catch (Exception ex)
            {
                // Internal server error
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = (object)null
                });
            }
        }
    }
}
