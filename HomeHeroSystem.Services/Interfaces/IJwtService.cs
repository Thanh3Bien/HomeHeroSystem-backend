using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(AppUser user);
        string GenerateAccessToken(Technician technician);
        bool ValidateToken(string token);
        Dictionary<string, string> GetTokenClaims(string token);
    }
}
