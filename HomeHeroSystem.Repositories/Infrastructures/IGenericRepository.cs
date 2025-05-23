using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Repositories.Infrastructures
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        TEntity AddEntity(TEntity entity);
        void UpdateEntity(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
