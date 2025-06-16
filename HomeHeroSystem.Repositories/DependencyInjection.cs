using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using HomeHeroSystem.Repositories.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHeroSystem.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDALServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
