using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.MinimalApi.DTOs;

namespace WarehouseManagementSystem.MinimalApi.Services;

public class CategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public List<CategoryResponse> GetAll()
    {
        return _context.Categories
            .Select(c => new CategoryResponse(
                c.CategoryId,
                c.CategoryName,
                c.Description,
                c.CreatedAt,
                c.UpdatedAt,
                c.Products.Count
            ))
            .ToList();
    }

    public CategoryResponse? GetById(int id)
    {
        var category = _context.Categories
            .Include(c => c.Products)
            .FirstOrDefault(c => c.CategoryId == id);

        if (category == null) return null;

        return new CategoryResponse(
            category.CategoryId,
            category.CategoryName,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt,
            category.Products.Count
        );
    }

    public CategoryResponse Create(CreateCategoryRequest request)
    {
        var category = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.Categories.Add(category);
        _context.SaveChanges();

        return new CategoryResponse(
            category.CategoryId,
            category.CategoryName,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt,
            0
        );
    }

    public CategoryResponse? Update(int id, UpdateCategoryRequest request)
    {
        var category = _context.Categories.Find(id);
        if (category == null) return null;

        if (request.CategoryName != null) category.CategoryName = request.CategoryName;
        if (request.Description != null) category.Description = request.Description;
        category.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        return new CategoryResponse(
            category.CategoryId,
            category.CategoryName,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt,
            category.Products.Count
        );
    }

    public bool Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return true;
    }
}
