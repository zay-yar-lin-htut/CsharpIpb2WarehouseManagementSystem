using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.ControllerApi.DTOs;
using PickAPile.Helpers;

namespace WarehouseManagementSystem.ControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var products = context.Products
            .Include(p => p.Category)
            .Select(p => new ProductResponse(
                p.ProductId,
                p.ProductName,
                p.Sku,
                p.Quantity,
                p.UnitPrice,
                p.Description,
                p.CategoryId,
                p.Category != null ? p.Category.CategoryName : null,
                p.CreatedAt
            ))
            .ToList();

        return Common.Success(products, 200, "Products retrieved successfully");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var product = context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductId == id);

        if (product == null)
            return Common.Error(404, "Product not found");

        var response = new ProductResponse(
            product.ProductId,
            product.ProductName,
            product.Sku,
            product.Quantity,
            product.UnitPrice,
            product.Description,
            product.CategoryId,
            product.Category?.CategoryName,
            product.CreatedAt
        );

        return Common.Success(response, 200, "Product retrieved successfully");
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateProductRequest request)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        if (request.CategoryId.HasValue)
        {
            var categoryExists = context.Categories.Any(c => c.CategoryId == request.CategoryId);
            if (!categoryExists)
                return Common.Error(400, "Category not found");
        }

        if (!string.IsNullOrEmpty(request.Sku))
        {
            var skuExists = context.Products.Any(p => p.Sku == request.Sku);
            if (skuExists)
                return Common.Error(400, "Sku already exists");
        }

        var product = new Product
        {
            ProductName = request.ProductName,
            Sku = request.Sku,
            Quantity = request.Quantity ?? 0,
            UnitPrice = request.UnitPrice,
            Description = request.Description,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.Now
        };

        context.Products.Add(product);
        context.SaveChanges();

        var response = new ProductResponse(
            product.ProductId,
            product.ProductName,
            product.Sku,
            product.Quantity,
            product.UnitPrice,
            product.Description,
            product.CategoryId,
            null,
            product.CreatedAt
        );

        return Common.Success(response, 201, "Product created successfully");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateProductRequest request)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var product = context.Products.Find(id);
        if (product == null)
            return Common.Error(404, "Product not found");

        if (request.CategoryId.HasValue)
        {
            var categoryExists = context.Categories.Any(c => c.CategoryId == request.CategoryId);
            if (!categoryExists)
                return Common.Error(400, "Category not found");
        }

        if (!string.IsNullOrEmpty(request.Sku))
        {
            var skuExists = context.Products.Any(p => p.Sku == request.Sku && p.ProductId != id);
            if (skuExists)
                return Common.Error(400, "Sku already exists");
        }

        if (request.ProductName != null) product.ProductName = request.ProductName;
        if (request.Sku != null) product.Sku = request.Sku;
        if (request.Quantity.HasValue) product.Quantity = request.Quantity;
        if (request.UnitPrice.HasValue) product.UnitPrice = request.UnitPrice;
        if (request.Description != null) product.Description = request.Description;
        if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId;

        context.SaveChanges();

        var category = product.CategoryId.HasValue
            ? context.Categories.Find(product.CategoryId)
            : null;

        var response = new ProductResponse(
            product.ProductId,
            product.ProductName,
            product.Sku,
            product.Quantity,
            product.UnitPrice,
            product.Description,
            product.CategoryId,
            category?.CategoryName,
            product.CreatedAt
        );

        return Common.Success(response, 200, "Product updated successfully");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var product = context.Products.Find(id);
        if (product == null)
            return Common.Error(404, "Product not found");

        context.Products.Remove(product);
        context.SaveChanges();

        return Common.Success(null, 204, "Product deleted successfully");
    }
}
