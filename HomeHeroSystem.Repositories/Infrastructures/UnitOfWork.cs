using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Interfaces;
using HomeHeroSystem.Repositories.Repositories;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeHeroContext _context;
        private readonly ILogger _logger;
        
        public IAdminRepository Admins { get; private set; }
        public IAppUserRepository AppUsers { get; private set; }

        public IBookingRepository Bookings { get; private set; }   
        public ITechnicianRepository Technicians { get; private set; }
        public IServiceRepository Services { get; private set; }
        public UnitOfWork(HomeHeroContext context, ILoggerFactory loggerFactory)
        {
            _context = context;

            _logger = loggerFactory.CreateLogger("logs");

            Admins = new AdminRepository(_context, _logger);

            AppUsers = new AppUserRepository(_context, _logger);

            Bookings = new BookingRepository(_context, _logger); 
            Technicians = new TechnicianRepository(_context, _logger);
            Services = new ServiceRepository(_context, _logger);    

        }
        public async Task CompleteAsync() => await _context.SaveChangesAsync();
    }
}
