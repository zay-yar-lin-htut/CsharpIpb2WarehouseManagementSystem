using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.ControllerApi.DTOs;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "CategoryName is required")]
    [StringLength(100, ErrorMessage = "CategoryName cannot exceed 100 characters")]
    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class UpdateCategoryRequest
{
    [StringLength(100, ErrorMessage = "CategoryName cannot exceed 100 characters")]
    public string? CategoryName { get; set; }

    public string? Description { get; set; }
}

public record CategoryResponse(
    int CategoryId,
    string CategoryName,
    string? Description,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    int ProductCount
);
