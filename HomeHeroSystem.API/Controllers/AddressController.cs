using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Address;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts()
        {
            try
            {
                var districts = await _addressService.GetDistrictsAsync();
                return Ok(districts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("wards")]
        public async Task<IActionResult> GetWards([FromQuery] string district)
        {
            try
            {
                if (string.IsNullOrEmpty(district))
                {
                    return BadRequest(new { message = "District is required" });
                }

                var wards = await _addressService.GetWardsByDistrictAsync(district);
                return Ok(wards);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrGetAddress([FromBody] CreateAddressRequest request)
        {
            try
            {
                var result = await _addressService.CreateOrGetAddressAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(id);
                if (address == null)
                {
                    return NotFound(new { message = "Address not found" });
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAddress([FromQuery] string street, [FromQuery] string ward, [FromQuery] string district, [FromQuery] string city = "Ho Chi Minh City")
        {
            try
            {
                var address = await _addressService.FindAddressAsync(street, ward, district, city);
                if (address == null)
                {
                    return NotFound(new { message = "Address not found" });
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
