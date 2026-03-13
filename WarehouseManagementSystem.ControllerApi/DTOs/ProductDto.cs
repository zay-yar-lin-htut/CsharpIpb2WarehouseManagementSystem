using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.ControllerApi.DTOs;

public class CreateProductRequest
{
    [Required(ErrorMessage = "ProductName is required")]
    [StringLength(100, ErrorMessage = "ProductName cannot exceed 100 characters")]
    public string ProductName { get; init; } = string.Empty;

    [StringLength(50, ErrorMessage = "Sku cannot exceed 50 characters")]
    public string? Sku { get; init; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
    public int? Quantity { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "UnitPrice must be 0 or greater")]
    public decimal? UnitPrice { get; init; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive number")]
    public int? CategoryId { get; init; }
}

public class UpdateProductRequest
{
    [StringLength(100, ErrorMessage = "ProductName cannot exceed 100 characters")]
    public string? ProductName { get; init; }

    [StringLength(50, ErrorMessage = "Sku cannot exceed 50 characters")]
    public string? Sku { get; init; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
    public int? Quantity { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "UnitPrice must be 0 or greater")]
    public decimal? UnitPrice { get; init; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive number")]
    public int? CategoryId { get; init; }
}

public record ProductResponse(
    int ProductId,
    string ProductName,
    string? Sku,
    int? Quantity,
    decimal? UnitPrice,
    string? Description,
    int? CategoryId,
    string? CategoryName,
    DateTime? CreatedAt
);
