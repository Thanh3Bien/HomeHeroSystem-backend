using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Product;
using HomeHeroSystem.Services.Models.ProductCategory;

namespace HomeHeroSystem.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetProductResponse> GetProductsAsync(GetProductRequest request)
        {
            var (products, totalCount) = await _unitOfWork.Products.GetProductsWithFiltersAsync(
                request.PageNumber, request.PageSize, request.SearchKeyword, request.CategoryId,
                request.Brand, request.MinPrice, request.MaxPrice, request.SortBy, request.SortDirection);

            var categories = await _unitOfWork.ProductCategories.GetActiveCategoriesAsync();
            var brands = await _unitOfWork.Products.GetActiveBrandsAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            // Map categories with product count
            var categoryDtos = new List<GetProductCategoryResponse>();
            foreach (var category in categories)
            {
                var productCount = await _unitOfWork.ProductCategories.GetProductCountByCategoryAsync(category.CategoryId);
                categoryDtos.Add(new GetProductCategoryResponse
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    IconUrl = category.IconUrl,
                    ProductCount = productCount
                });
            }

            return new GetProductResponse
            {
                Products = products.Select(p => new ProductItem
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    ShortDescription = p.ShortDescription,
                    Price = p.Price,
                    Brand = p.Brand,
                    StockQuantity = p.StockQuantity,
                    Unit = p.Unit,
                    ImageUrl = p.ImageUrl,
                    Sku = p.Sku,
                    IsInStock = (p.StockQuantity ?? 0) > (p.MinStockLevel ?? 0)
                }).ToList(),
                TotalItems = totalCount,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber,
                Categories = categoryDtos,
                Brands = brands
            };
        }
        public async Task<GetProductDetailResponse> GetProductDetailAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetProductDetailAsync(productId);
            if (product == null)
                throw new ArgumentException("Product not found");

            var relatedProducts = await _unitOfWork.Products.GetRelatedProductAsync(
                productId, product.CategoryId);

            return new GetProductDetailResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryName = product.Category.CategoryName,
                ShortDescription = product.ShortDescription,
                DetailedDescription = product.DetailedDescription,
                Price = product.Price,
                Brand = product.Brand,
                StockQuantity = product.StockQuantity,
                Unit = product.Unit,
                ImageUrl = product.ImageUrl,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                Sku = product.Sku,
                IsInStock = (product.StockQuantity ?? 0) > (product.MinStockLevel ?? 0),
                RelatedProducts = relatedProducts.Select(p => new ProductItem
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    ShortDescription = p.ShortDescription,
                    Price = p.Price,
                    Brand = p.Brand,
                    Unit = p.Unit,
                    ImageUrl = p.ImageUrl,
                    IsInStock = (p.StockQuantity ?? 0) > (p.MinStockLevel ?? 0)
                }).ToList()
            };
        }
    }
}
