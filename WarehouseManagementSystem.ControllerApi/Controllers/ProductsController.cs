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
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _context.Products
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
        var product = _context.Products
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

        _context.Products.Add(product);
        _context.SaveChanges();

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
        var product = _context.Products.Find(id);
        if (product == null)
            return Common.Error(404, "Product not found");

        if (request.ProductName != null) product.ProductName = request.ProductName;
        if (request.Sku != null) product.Sku = request.Sku;
        if (request.Quantity.HasValue) product.Quantity = request.Quantity;
        if (request.UnitPrice.HasValue) product.UnitPrice = request.UnitPrice;
        if (request.Description != null) product.Description = request.Description;
        if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId;

        _context.SaveChanges();

        var category = product.CategoryId.HasValue
            ? _context.Categories.Find(product.CategoryId)
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
        var product = _context.Products.Find(id);
        if (product == null)
            return Common.Error(404, "Product not found");

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Common.Success(null, 204, "Product deleted successfully");
    }
}
