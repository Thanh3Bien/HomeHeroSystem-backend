using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Interfaces;

namespace HomeHeroSystem.Repositories.Infrastructures
{
    public interface IUnitOfWork
    {
        IAdminRepository Admins { get; }
        IAppUserRepository AppUsers { get; }

        IBookingRepository Bookings { get; }
        ITechnicianRepository Technicians { get; }
        ITechnicianSkillRepository TechnicianSkills { get; }
        IServiceRepository Services { get; }
        ISkillRepository Skills { get; }

        IProductRepository Products { get; }
        IProductCategoryRepository ProductCategories { get; }
        IAddressRepository Addresses { get; }
        IServiceCategoryRepository ServiceCategories { get; }
        IPaymentRepository Payments { get; }

        Task CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
