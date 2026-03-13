using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Database.AppDbContextModels;

public partial class Storer
{
    public int StorerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }
}
