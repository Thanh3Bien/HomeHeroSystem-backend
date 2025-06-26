using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HomeHeroSystem.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["JWT:SecretKey"] ?? throw new ArgumentNullException("JWT:SecretKey");
            _issuer = _configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT:Issuer");
            _audience = _configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT:Audience");
            _expireMinutes = int.Parse(_configuration["JWT:AccessTokenExpireMinutes"] ?? "60");
        }

        public string GenerateAccessToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Phone", user.Phone),
                new Claim("UserType", "User"),
                new Claim("AddressId", user.AddressId?.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            return GenerateToken(claims);
        }

        public string GenerateAccessToken(Technician technician)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, technician.TechnicianId.ToString()),
                new Claim(ClaimTypes.Name, technician.FullName),
                new Claim(ClaimTypes.Email, technician.Email),
                new Claim("Phone", technician.Phone),
                new Claim("UserType", "Technician"),
                new Claim("ExperienceYears", technician.ExperienceYears?.ToString() ?? "0"),
                new Claim("AddressId", technician.AddressId?.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add skills if available
            if (technician.TechnicianSkills != null && technician.TechnicianSkills.Any())
            {
                var skills = string.Join(",", technician.TechnicianSkills.Select(ts => ts.Skill?.SkillName ?? ""));
                claims.Add(new Claim("Skills", skills));
            }

            return GenerateToken(claims);
        }

        private string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Dictionary<string, string> GetTokenClaims(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                return jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
