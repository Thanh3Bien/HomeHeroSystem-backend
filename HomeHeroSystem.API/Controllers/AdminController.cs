using HomeHeroSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                _logger.LogInformation("GetAllAdmins endpoint called");

                var admins = await _adminService.GetAllAdminsAsync();

                if (admins == null || !admins.Any())
                {
                    _logger.LogInformation("No admins found");
                    return Ok(new { message = "No admins found", data = new List<object>() });
                }

                _logger.LogInformation($"Successfully retrieved {admins.Count()} admins");
                return Ok(new { message = "Admins retrieved successfully", data = admins });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all admins");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
    }
}
