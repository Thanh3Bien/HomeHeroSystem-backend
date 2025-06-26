using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Technician;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianController : ControllerBase
    {
        private readonly ITechnicianService _technicianService;
        public TechnicianController(ITechnicianService technicianService)
        {
            _technicianService = technicianService; 
        }
        [HttpGet]
        public async Task<IActionResult> GetTechniciansAsync([FromQuery] GetTechnicianRequest request)
        {
            try
            {
                var result = await _technicianService.GetTechnicianAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTechnicianAsync([FromBody] CreateTechnicianRequest createTechnicianRequest)
        {
            try
            {
                var result = await _technicianService.CreateTechnicianAsync(createTechnicianRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechnicianAsync(int id, [FromBody] UpdateTechnicianRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid technician ID" });
                }

                var result = await _technicianService.UpdateTechnicianAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnicianAsync(int id)
        {
            try
            {
                var result = await _technicianService.DeleteTechnicianAsync(id);
                return Ok(result);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTechnicianStatusAsync(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var result = await _technicianService.UpdateTechnicianStatusAsync(id, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetTechnicianStatisticsAsync()
        {
            try
            {
                var result = await _technicianService.GetTechnicianStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<string>>> GetTechnicianNames()
        {
            try
            {
                var technicianNames = await _technicianService.GetTechnicianNamesAsync();
                return Ok(technicianNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<string>>> SearchTechnicianNames([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    // If no keyword, return all technician names
                    var allTechnicianNames = await _technicianService.GetTechnicianNamesAsync();
                    return Ok(allTechnicianNames);
                }

                var filteredTechnicianNames = await _technicianService.SearchTechnicianNamesAsync(keyword);
                return Ok(filteredTechnicianNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTechnicianByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid technician ID. ID must be greater than 0." });
                }


                var result = await _technicianService.GetTechnicianByIdAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
