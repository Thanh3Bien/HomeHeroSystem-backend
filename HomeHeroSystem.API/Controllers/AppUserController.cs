using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.AppUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ILogger<AppUserController> _logger;
        public AppUserController(IAppUserService appUserService, ILogger<AppUserController> logger)
        {
            _appUserService = appUserService;
            _logger = logger;   
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _appUserService.GetAllUsersAsync();
                return Ok(new { message = "Users retrieved successfully", data = users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all users");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _appUserService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(new { message = "User retrieved successfully", data = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting user by id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _appUserService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(new { message = "User retrieved successfully", data = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting user by email: {email}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (users, totalCount) = await _appUserService.GetActiveUsersAsync(page, pageSize);
                return Ok(new
                {
                    message = "Active users retrieved successfully",
                    data = users,
                    pagination = new { page, pageSize, totalCount, totalPages = (int)Math.Ceiling((double)totalCount / pageSize) }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting active users");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return BadRequest(new { message = "Search term is required" });
                }

                var users = await _appUserService.SearchUsersAsync(term);
                return Ok(new { message = "Search completed successfully", data = users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching users with term: {term}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdUser = await _appUserService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId },
                    new { message = "User created successfully", data = createdUser });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AppUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedUser = await _appUserService.UpdateUserAsync(id, user);
                if (updatedUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new { message = "User updated successfully", data = updatedUser });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating user with id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            try
            {
                var result = await _appUserService.DeactivateUserAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new { message = "User deactivated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deactivating user with id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            try
            {
                var result = await _appUserService.ActivateUserAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new { message = "User activated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while activating user with id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        [HttpGet("{id}/personal-information")]
        public async Task<IActionResult> GetPersonalInformation(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid user ID" });
                }

                var personalInfo = await _appUserService.GetPersonalInformationAsync(id);
                if (personalInfo == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    message = "Personal information retrieved successfully",
                    data = personalInfo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting personal information for user id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        [HttpPut("{id}/personal-information")]
        public async Task<IActionResult> UpdatePersonalInformation(int id, [FromBody] UpdatePersonalInformationRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid user ID" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedInfo = await _appUserService.UpdatePersonalInformationAsync(id, request);
                if (updatedInfo == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    message = "Personal information updated successfully",
                    data = updatedInfo
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating personal information for user id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid user ID" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _appUserService.ChangePasswordAsync(id, request);

                if (result.Success)
                {
                    return Ok(new
                    {
                        message = result.Message,
                        data = result
                    });
                }
                else
                {
                    return BadRequest(new { message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while changing password for user id: {id}");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
    }
}
