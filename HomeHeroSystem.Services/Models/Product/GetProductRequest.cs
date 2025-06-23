using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Product
{
    public class GetProductRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public string? SearchKeyword { get; set; }
        public int? CategoryId { get; set; }
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } = "name"; // name, price, brand
        public string? SortDirection { get; set; } = "asc"; // asc, desc
    }
}
