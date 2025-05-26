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

                var result = await _authenticationService.LoginAsync(loginRequest);

                if (result == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                return Ok(new
                {
                    message = "Login successful",
                    data = result,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login");
                return StatusCode(500, new { message = "Internal server error occurred" });
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

                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid or missing user token" });
                }

                var result = await _authenticationService.LogoutAsync(userId);

                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    message = "Logout successful",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout");
                return StatusCode(500, new { message = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Get current user information from JWT token
        /// </summary>
        /// <returns>Current user profile information</returns>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var nameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
                var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
                var phoneClaim = User.FindFirst("Phone")?.Value;
                var userTypeClaim = User.FindFirst("UserType")?.Value;

                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                return Ok(new
                {
                    message = "User information retrieved successfully",
                    data = new
                    {
                        UserId = userId,
                        FullName = nameClaim,
                        Email = emailClaim,
                        Phone = phoneClaim,
                        UserType = userTypeClaim
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting current user info");
                return StatusCode(500, new { message = "Internal server error occurred" });
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

