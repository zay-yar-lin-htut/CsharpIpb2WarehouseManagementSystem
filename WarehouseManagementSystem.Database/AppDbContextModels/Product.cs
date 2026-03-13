using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Database.AppDbContextModels;

public partial class Product
{
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Sku { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }
}
