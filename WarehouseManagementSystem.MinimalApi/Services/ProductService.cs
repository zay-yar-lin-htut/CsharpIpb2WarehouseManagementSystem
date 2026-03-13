using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.MinimalApi.DTOs;

namespace WarehouseManagementSystem.MinimalApi.Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public List<ProductResponse> GetAll()
    {
        return _context.Products
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
    }

    public ProductResponse? GetById(int id)
    {
        var product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductId == id);

        if (product == null) return null;

        return new ProductResponse(
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
    }

    public ProductResponse Create(CreateProductRequest request)
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

        return new ProductResponse(
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
    }

    public ProductResponse? Update(int id, UpdateProductRequest request)
    {
        var product = _context.Products.Find(id);
        if (product == null) return null;

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

        return new ProductResponse(
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
    }

    public bool Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        _context.SaveChanges();
        return true;
    }
}
