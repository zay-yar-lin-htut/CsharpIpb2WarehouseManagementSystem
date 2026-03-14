using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.MinimalApi.DTOs;

public class CreateWarehouseRequest
{
    [Required(ErrorMessage = "WarehouseName is required")]
    [StringLength(100, ErrorMessage = "WarehouseName cannot exceed 100 characters")]
    public string WarehouseName { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters")]
    public string? Location { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
    public int? Capacity { get; set; }
}

public class UpdateWarehouseRequest
{
    [StringLength(100, ErrorMessage = "WarehouseName cannot exceed 100 characters")]
    public string? WarehouseName { get; set; }

    [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters")]
    public string? Location { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
    public int? Capacity { get; set; }
}

public record WarehouseResponse(
    int WarehouseId,
    string WarehouseName,
    string? Location,
    int? Capacity,
    DateTime? CreatedAt
);
