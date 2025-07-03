using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Interfaces;
using HomeHeroSystem.Repositories.Repositories;
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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}
