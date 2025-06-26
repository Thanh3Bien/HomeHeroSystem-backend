using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }
        /// <summary>
        /// Login with email and password
        /// </summary>
        /// <param name="loginRequest">Login credentials (email and password)</param>
        /// <returns>JWT access token and user information</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid input data",
                        errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                            .ToDictionary(x => x.Key, x => x.Value?.Errors.Select(e => e.ErrorMessage))
                    });
                }

                // Validate user type
                var validUserTypes = new[] { "User", "Technician" };
                if (!validUserTypes.Contains(loginRequest.UserType, StringComparer.OrdinalIgnoreCase))
                {
                    return BadRequest(new
                    {
                        message = "Invalid user type. Must be 'User' or 'Technician'"
                    });
                }

                var result = await _authenticationService.LoginAsync(loginRequest);

                if (result == null)
                {
                    return Unauthorized(new
                    {
                        message = $"Invalid email, password, or user type. Please check your credentials and try again."
                    });
                }

                return Ok(new
                {
                    IsSuccess = true,
                    Message = $"Login successful as {result.UserType}",
                    Data = result,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login");
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "Internal server error occurred",
                    Data = (object)null
                });
            }
        }

        /// <summary>
        /// Logout current user (clears server-side tracking)
        /// </summary>
        /// <returns>Success message</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userTypeClaim = User.FindFirst("UserType")?.Value;

                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId) || string.IsNullOrEmpty(userTypeClaim))
                {
                    return Unauthorized(new
                    {
                        IsSuccess = false,
                        Message = "Invalid or missing user token"
                    });
                }

                var result = await _authenticationService.LogoutAsync(userId, userTypeClaim);

                if (!result)
                {
                    return NotFound(new
                    {
                        IsSuccess = false,
                        Message = "User not found"
                    });
                }

                return Ok(new
                {
                    IsSuccess = true,
                    Message = $"Logout successful for {userTypeClaim}",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout");
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "Internal server error occurred"
                });
            }
        }

        /// <summary>
        /// Get current user information from JWT token
        /// </summary>
        /// <returns>Current user profile information</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userTypeClaim = User.FindFirst("UserType")?.Value;

                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId) || string.IsNullOrEmpty(userTypeClaim))
                {
                    return Unauthorized(new
                    {
                        IsSuccess = false,
                        Message = "Invalid token"
                    });
                }

                // Get detailed user information from database
                var currentUser = await _authenticationService.GetCurrentUserAsync(userId, userTypeClaim);

                if (currentUser == null)
                {
                    return NotFound(new
                    {
                        IsSuccess = false,
                        Message = "User not found"
                    });
                }

                return Ok(new
                {
                    IsSuccess = true,
                    Message = "User information retrieved successfully",
                    Data = currentUser,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting current user info");
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "Internal server error occurred"
                });
            }
        }

        /// <summary>
        /// Validate JWT token (useful for frontend token validation)
        /// </summary>
        /// <returns>Token validation status</returns>
        [HttpGet("validate-token")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                return Ok(new
                {
                    message = "Token is valid",
                    data = new
                    {
                        UserId = userId,
                        IsValid = true,
                        ExpiryTime = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token validation");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }
    }
}

