using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<(List<Product> products, int totalCount)> GetProductsWithFiltersAsync(
            int pageNumber, int pageSize, string? searchKeyword, int? categoryId,
            string? brand, decimal? minPrice, decimal? maxPrice,
            string? sortBy, string? sortDirection);
        Task<Product?> GetProductDetailAsync(int productId); 
        Task<List<Product>> GetRelatedProductAsync(int productId, int categoryId, int limit = 4);
        Task<List<string>> GetActiveBrandsAsync();
    }
}
