using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.ProductCategory;

namespace HomeHeroSystem.Services.Models.Product
{
    public class GetProductResponse
    {
        public List<ProductItem> Products { get; set; } = new();
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<GetProductCategoryResponse> Categories { get; set; } = new();
        public List<string> Brands { get; set; } = new();
    }
}
