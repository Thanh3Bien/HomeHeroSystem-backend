using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Interfaces;
using HomeHeroSystem.Repositories.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeHeroContext _context;
        private readonly ILogger _logger;
        private IDbContextTransaction? _transaction;

        public IAdminRepository Admins { get; private set; }
        public IAppUserRepository AppUsers { get; private set; }

        public IBookingRepository Bookings { get; private set; }   
        public ITechnicianRepository Technicians { get; private set; }
        public ITechnicianSkillRepository TechnicianSkills { get; private set; }
        public IServiceRepository Services { get; private set; }
        public ISkillRepository Skills { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductCategoryRepository ProductCategories { get; private set; }
        public IAddressRepository Addresses {  get; private set; } 
        public IServiceCategoryRepository ServiceCategories { get; private set; }
        public UnitOfWork(HomeHeroContext context, ILoggerFactory loggerFactory)
        {
            _context = context;

            _logger = loggerFactory.CreateLogger("logs");

            Admins = new AdminRepository(_context, _logger);

            AppUsers = new AppUserRepository(_context, _logger);

            Bookings = new BookingRepository(_context, _logger); 
            Technicians = new TechnicianRepository(_context, _logger);
            TechnicianSkills = new TechnicianSkillRepository(_context, _logger);
            Services = new ServiceRepository(_context, _logger);
            Skills = new SkillRepository(_context, _logger);
            Products = new ProductRepository(_context, _logger);
            ProductCategories = new ProductCategoryRepository(_context, _logger);
            Addresses = new AddressRepository(_context, _logger);
            ServiceCategories = new ServiceCategoryRepository(_context, _logger);

        }
        public async Task CompleteAsync() => await _context.SaveChangesAsync();

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if(_transaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
