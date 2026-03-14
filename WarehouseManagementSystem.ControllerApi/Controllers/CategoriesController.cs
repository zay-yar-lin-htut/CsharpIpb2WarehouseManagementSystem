using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.ControllerApi.DTOs;
using PickAPile.Helpers;

namespace WarehouseManagementSystem.ControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var categories = context.Categories
            .Select(c => new CategoryResponse(
                c.CategoryId,
                c.CategoryName,
                c.Description,
                c.CreatedAt,
                c.UpdatedAt,
                c.Products.Count
            ))
            .ToList();

        return Common.Success(categories, 200, "Categories retrieved successfully");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var category = context.Categories
            .Include(c => c.Products)
            .FirstOrDefault(c => c.CategoryId == id);

        if (category == null)
            return Common.Error(404, "Category not found");

        var response = new CategoryResponse(
            category.CategoryId,
            category.CategoryName,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt,
            category.Products.Count
        );

        return Common.Success(response, 200, "Category retrieved successfully");
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateCategoryRequest request)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var category = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        context.Categories.Add(category);
        context.SaveChanges();

        var response = new CategoryResponse(
            category.CategoryId,
            category.CategoryName,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt,
            0
        );

        return Common.Success(response, 201, "Category created successfully");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateCategoryRequest request)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var category = context.Categories.Find(id);
        if (category == null)
            return Common.Error(404, "Category not found");

        if (request.CategoryName != null) category.CategoryName = request.CategoryName;
        if (request.Description != null) category.Description = request.Description;
        category.UpdatedAt = DateTime.Now;

        context.SaveChanges();

        var response = new CategoryResponse(
            category.CategoryId,
            category.CategoryName,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt,
            category.Products.Count
        );

        return Common.Success(response, 200, "Category updated successfully");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var category = context.Categories.Find(id);
        if (category == null)
            return Common.Error(404, "Category not found");

        context.Categories.Remove(category);
        context.SaveChanges();

        return Common.Success(null, 204, "Category deleted successfully");
    }
}
