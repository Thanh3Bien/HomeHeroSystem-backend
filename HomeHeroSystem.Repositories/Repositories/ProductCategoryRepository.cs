using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Repositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public async Task<List<ProductCategory>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive == true && c.IsDeleted != true)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<int> GetProductCountByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .CountAsync(p => p.CategoryId == categoryId &&
                                p.IsActive == true &&
                                p.IsDeleted != true);
        }
    }
}
