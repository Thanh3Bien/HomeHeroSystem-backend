using HomeHeroSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<string>>> GetServiceNames()
        {
            try
            {
                var serviceNames = await _serviceService.GetServiceNamesAsync();
                return Ok(serviceNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<string>>> SearchServiceNames([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {

                    var allServiceNames = await _serviceService.GetServiceNamesAsync();
                    return Ok(allServiceNames);
                }

                var filteredServiceNames = await _serviceService.SearchServiceNamesAsync(keyword);
                return Ok(filteredServiceNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetServicesByCategory(int categoryId)
        {
            

            try
            {
                var result = await _serviceService.GetServicesByCategoryAsync(categoryId);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

       
        }

        [HttpGet("{serviceId}/price")]
        public async Task<IActionResult> GetServicePrice(int serviceId, [FromQuery] string urgencyLevel = "normal")
        {
            

            try
            {
                var result = await _serviceService.GetServicePriceAsync(serviceId, urgencyLevel);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetService(int serviceId)
        {
            

            try
            {
                var result = await _serviceService.GetServiceByIdAsync(serviceId);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
