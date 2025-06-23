using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.ProductCategory
{
    public class GetProductCategoryResponse
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public int ProductCount { get; set; }
    }
}
