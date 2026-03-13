using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.MinimalApi.DTOs;
using WarehouseManagementSystem.MinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<StorerService>();

var app = builder.Build();

static (bool isValid, List<string> errors) Validate<T>(T request)
{
    var validationContext = new ValidationContext(request);
    var validationResults = new List<ValidationResult>();
    bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

    if (!isValid)
    {
        var errors = validationResults.Select(v => v.ErrorMessage).ToList();
        return (false, errors);
    }
    return (true, new List<string>());
}

var products = app.MapGroup("/api/products");
products.MapGet("/", (ProductService service) => Results.Ok(service.GetAll()));
products.MapGet("/{id}", (int id, ProductService service) =>
{
    var product = service.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});
products.MapPost("/", (CreateProductRequest request, ProductService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var product = service.Create(request);
    return Results.Created($"/api/products/{product.ProductId}", product);
});
products.MapPut("/{id}", (int id, UpdateProductRequest request, ProductService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var product = service.Update(id, request);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});
products.MapDelete("/{id}", (int id, ProductService service) =>
{
    var result = service.Delete(id);
    return result ? Results.NoContent() : Results.NotFound();
});

var categories = app.MapGroup("/api/categories");
categories.MapGet("/", (CategoryService service) => Results.Ok(service.GetAll()));
categories.MapGet("/{id}", (int id, CategoryService service) =>
{
    var category = service.GetById(id);
    return category is not null ? Results.Ok(category) : Results.NotFound();
});
categories.MapPost("/", (CreateCategoryRequest request, CategoryService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var category = service.Create(request);
    return Results.Created($"/api/categories/{category.CategoryId}", category);
});
categories.MapPut("/{id}", (int id, UpdateCategoryRequest request, CategoryService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var category = service.Update(id, request);
    return category is not null ? Results.Ok(category) : Results.NotFound();
});
categories.MapDelete("/{id}", (int id, CategoryService service) =>
{
    var result = service.Delete(id);
    return result ? Results.NoContent() : Results.NotFound();
});

var warehouses = app.MapGroup("/api/warehouses");
warehouses.MapGet("/", (WarehouseService service) => Results.Ok(service.GetAll()));
warehouses.MapGet("/{id}", (int id, WarehouseService service) =>
{
    var warehouse = service.GetById(id);
    return warehouse is not null ? Results.Ok(warehouse) : Results.NotFound();
});
warehouses.MapPost("/", (CreateWarehouseRequest request, WarehouseService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var warehouse = service.Create(request);
    return Results.Created($"/api/warehouses/{warehouse.WarehouseId}", warehouse);
});
warehouses.MapPut("/{id}", (int id, UpdateWarehouseRequest request, WarehouseService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var warehouse = service.Update(id, request);
    return warehouse is not null ? Results.Ok(warehouse) : Results.NotFound();
});
warehouses.MapDelete("/{id}", (int id, WarehouseService service) =>
{
    var result = service.Delete(id);
    return result ? Results.NoContent() : Results.NotFound();
});

var storers = app.MapGroup("/api/storers");
storers.MapGet("/", (StorerService service) => Results.Ok(service.GetAll()));
storers.MapGet("/{id}", (int id, StorerService service) =>
{
    var storer = service.GetById(id);
    return storer is not null ? Results.Ok(storer) : Results.NotFound();
});
storers.MapPost("/", (CreateStorerRequest request, StorerService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var storer = service.Create(request);
    return Results.Created($"/api/storers/{storer.StorerId}", storer);
});
storers.MapPut("/{id}", (int id, UpdateStorerRequest request, StorerService service) =>
{
    var (isValid, errors) = Validate(request);
    if (!isValid)
        return Results.BadRequest(new { errors });
    
    var storer = service.Update(id, request);
    return storer is not null ? Results.Ok(storer) : Results.NotFound();
});
storers.MapDelete("/{id}", (int id, StorerService service) =>
{
    var result = service.Delete(id);
    return result ? Results.NoContent() : Results.NotFound();
});

app.Run();

public partial class Program { }
