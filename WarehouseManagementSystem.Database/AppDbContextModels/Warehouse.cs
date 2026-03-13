using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Database.AppDbContextModels;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string? Location { get; set; }

    public int? Capacity { get; set; }

    public DateTime? CreatedAt { get; set; }
}
