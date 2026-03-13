using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.MinimalApi.DTOs;

public class CreateWarehouseRequest
{
    [Required(ErrorMessage = "WarehouseName is required")]
    [StringLength(100, ErrorMessage = "WarehouseName cannot exceed 100 characters")]
    public string WarehouseName { get; init; } = string.Empty;

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string? Location { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
    public int? Capacity { get; init; }
}

public class UpdateWarehouseRequest
{
    [StringLength(100, ErrorMessage = "WarehouseName cannot exceed 100 characters")]
    public string? WarehouseName { get; init; }

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string? Location { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
    public int? Capacity { get; init; }
}

public record WarehouseResponse(
    int WarehouseId,
    string WarehouseName,
    string? Location,
    int? Capacity,
    DateTime? CreatedAt
);
