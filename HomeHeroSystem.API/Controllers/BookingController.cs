using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Booking;
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
    }
}
