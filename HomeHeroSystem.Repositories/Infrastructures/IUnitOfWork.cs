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
        Task CompleteAsync();
    }
}
