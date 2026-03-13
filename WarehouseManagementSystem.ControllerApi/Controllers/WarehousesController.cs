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
    private readonly AppDbContext _context;

    public WarehousesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var warehouses = _context.Warehouses
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
        var warehouse = _context.Warehouses.Find(id);
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
        var warehouse = new Warehouse
        {
            WarehouseName = request.WarehouseName,
            Location = request.Location,
            Capacity = request.Capacity,
            CreatedAt = DateTime.Now
        };

        _context.Warehouses.Add(warehouse);
        _context.SaveChanges();

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
        var warehouse = _context.Warehouses.Find(id);
        if (warehouse == null)
            return Common.Error(404, "Warehouse not found");

        if (request.WarehouseName != null) warehouse.WarehouseName = request.WarehouseName;
        if (request.Location != null) warehouse.Location = request.Location;
        if (request.Capacity.HasValue) warehouse.Capacity = request.Capacity;

        _context.SaveChanges();

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
        var warehouse = _context.Warehouses.Find(id);
        if (warehouse == null)
            return Common.Error(404, "Warehouse not found");

        _context.Warehouses.Remove(warehouse);
        _context.SaveChanges();

        return Common.Success(null, 204, "Warehouse deleted successfully");
    }
}
