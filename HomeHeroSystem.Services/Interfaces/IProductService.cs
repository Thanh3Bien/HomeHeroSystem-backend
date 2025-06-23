using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Product;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IProductService
    {
        Task<GetProductResponse> GetProductsAsync(GetProductRequest request);
        Task<GetProductDetailResponse> GetProductDetailAsync(int productId);
    }
}
