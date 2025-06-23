using System;
using System.Collections.Generic;

namespace HomeHeroSystem.Repositories.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

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

    public int? MinStockLevel { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;
}
