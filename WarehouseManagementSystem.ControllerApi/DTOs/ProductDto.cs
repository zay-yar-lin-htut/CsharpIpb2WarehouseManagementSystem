using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.ControllerApi.DTOs;

public class CreateProductRequest
{
    [Required(ErrorMessage = "ProductName is required")]
    [StringLength(150, ErrorMessage = "ProductName cannot exceed 150 characters")]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Sku cannot exceed 50 characters")]
    public string? Sku { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
    public int? Quantity { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "UnitPrice must be 0 or greater")]
    public decimal? UnitPrice { get; set; }

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive number")]
    public int? CategoryId { get; set; }
}

public class UpdateProductRequest
{
    [StringLength(150, ErrorMessage = "ProductName cannot exceed 150 characters")]
    public string? ProductName { get; set; }

    [StringLength(50, ErrorMessage = "Sku cannot exceed 50 characters")]
    public string? Sku { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
    public int? Quantity { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "UnitPrice must be 0 or greater")]
    public decimal? UnitPrice { get; set; }

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive number")]
    public int? CategoryId { get; set; }
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
