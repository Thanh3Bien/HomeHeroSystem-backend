using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Mappings;
using HomeHeroSystem.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHeroSystem.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureBALServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ITechnicianService, TechnicianService>();
            services.AddScoped<ISkillService, SkillService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}
