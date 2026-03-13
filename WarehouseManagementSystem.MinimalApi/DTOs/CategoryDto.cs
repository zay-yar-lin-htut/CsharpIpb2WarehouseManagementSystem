using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.MinimalApi.DTOs;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "CategoryName is required")]
    [StringLength(100, ErrorMessage = "CategoryName cannot exceed 100 characters")]
    public string CategoryName { get; init; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; init; }
}

public class UpdateCategoryRequest
{
    [StringLength(100, ErrorMessage = "CategoryName cannot exceed 100 characters")]
    public string? CategoryName { get; init; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; init; }
}

public record CategoryResponse(
    int CategoryId,
    string CategoryName,
    string? Description,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    int ProductCount
);
