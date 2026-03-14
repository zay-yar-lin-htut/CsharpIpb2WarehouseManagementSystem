using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.ControllerApi.DTOs;
using PickAPile.Helpers;

namespace WarehouseManagementSystem.ControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var warehouses = context.Warehouses
            .Select(w => new WarehouseResponse(
                w.WarehouseId,
                w.WarehouseName,
                w.Location,
                w.Capacity,
                w.CreatedAt
            ))
            .ToList();

        return Common.Success(warehouses, 200, "Warehouses retrieved successfully");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var warehouse = context.Warehouses.Find(id);
        if (warehouse == null)
            return Common.Error(404, "Warehouse not found");

        var response = new WarehouseResponse(
            warehouse.WarehouseId,
            warehouse.WarehouseName,
            warehouse.Location,
            warehouse.Capacity,
            warehouse.CreatedAt
        );

        return Common.Success(response, 200, "Warehouse retrieved successfully");
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateWarehouseRequest request)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var warehouse = new Warehouse
        {
            WarehouseName = request.WarehouseName,
            Location = request.Location,
            Capacity = request.Capacity,
            CreatedAt = DateTime.Now
        };

        context.Warehouses.Add(warehouse);
        context.SaveChanges();

        var response = new WarehouseResponse(
            warehouse.WarehouseId,
            warehouse.WarehouseName,
            warehouse.Location,
            warehouse.Capacity,
            warehouse.CreatedAt
        );

        return Common.Success(response, 201, "Warehouse created successfully");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateWarehouseRequest request)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var warehouse = context.Warehouses.Find(id);
        if (warehouse == null)
            return Common.Error(404, "Warehouse not found");

        if (request.WarehouseName != null) warehouse.WarehouseName = request.WarehouseName;
        if (request.Location != null) warehouse.Location = request.Location;
        if (request.Capacity.HasValue) warehouse.Capacity = request.Capacity;

        context.SaveChanges();

        var response = new WarehouseResponse(
            warehouse.WarehouseId,
            warehouse.WarehouseName,
            warehouse.Location,
            warehouse.Capacity,
            warehouse.CreatedAt
        );

        return Common.Success(response, 200, "Warehouse updated successfully");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var warehouse = context.Warehouses.Find(id);
        if (warehouse == null)
            return Common.Error(404, "Warehouse not found");

        context.Warehouses.Remove(warehouse);
        context.SaveChanges();

        return Common.Success(null, 204, "Warehouse deleted successfully");
    }
}
