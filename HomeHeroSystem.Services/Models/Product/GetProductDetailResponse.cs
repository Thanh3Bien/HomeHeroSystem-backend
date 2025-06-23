using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Product
{
    public class GetProductDetailResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? DetailedDescription { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
        public int? StockQuantity { get; set; }
        public string Unit { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? Sku { get; set; }
        public bool IsInStock { get; set; }
        public List<ProductItem> RelatedProducts { get; set; } = new();
    }
}
