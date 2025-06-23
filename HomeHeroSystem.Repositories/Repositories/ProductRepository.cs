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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(HomeHeroContext context, ILogger logger)
            : base(context, logger)
        {
        }

        public async Task<List<string>> GetActiveBrandsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive == true &&
                            p.IsDeleted != true &&
                            !string.IsNullOrEmpty(p.Brand))
                .Select(p => p.Brand!)
                .Distinct()
                .OrderBy(b => b)
                .ToListAsync();
        }

        public async Task<Product?> GetProductDetailAsync(int productId)
        {
            return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == productId &&
                                   p.IsActive == true &&
                                   p.IsDeleted != true);
        }

        public async Task<(List<Product> products, int totalCount)> GetProductsWithFiltersAsync(int pageNumber, int pageSize, string? searchKeyword, int? categoryId, string? brand, decimal? minPrice, decimal? maxPrice, string? sortBy, string? sortDirection)
        {
            var query = _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsActive == true && p.IsDeleted != true);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(p => p.ProductName.Contains(searchKeyword) ||
                                   (p.ShortDescription != null && p.ShortDescription.Contains(searchKeyword)) ||
                                   (p.Brand != null && p.Brand.Contains(searchKeyword)));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // Brand filter
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Brand == brand);
            }

            // Price filter
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = sortBy?.ToLower() switch
            {
                "price" => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),
                "brand" => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.Brand)
                    : query.OrderBy(p => p.Brand),
                _ => sortDirection == "desc"
                    ? query.OrderByDescending(p => p.ProductName)
                    : query.OrderBy(p => p.ProductName)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<List<Product>> GetRelatedProductAsync(int productId, int categoryId, int limit = 4)
        {
            return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId &&
                       p.ProductId != productId &&
                       p.IsActive == true &&
                       p.IsDeleted != true)
            .OrderBy(p => p.ProductId)
            .Take(limit)
            .ToListAsync();
        }
    }
}
