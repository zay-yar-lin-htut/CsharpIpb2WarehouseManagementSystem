using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.MinimalApi.DTOs;

namespace WarehouseManagementSystem.MinimalApi.Services;

public class WarehouseService
{
    private readonly AppDbContext _context;

    public WarehouseService(AppDbContext context)
    {
        _context = context;
    }

    public List<WarehouseResponse> GetAll()
    {
        return _context.Warehouses
            .Select(w => new WarehouseResponse(
                w.WarehouseId,
                w.WarehouseName,
                w.Location,
                w.Capacity,
                w.CreatedAt
            ))
            .ToList();
    }

    public WarehouseResponse? GetById(int id)
    {
        var warehouse = _context.Warehouses.Find(id);
        if (warehouse == null) return null;

        return new WarehouseResponse(
            warehouse.WarehouseId,
            warehouse.WarehouseName,
            warehouse.Location,
            warehouse.Capacity,
            warehouse.CreatedAt
        );
    }

    public WarehouseResponse Create(CreateWarehouseRequest request)
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

        return new WarehouseResponse(
            warehouse.WarehouseId,
            warehouse.WarehouseName,
            warehouse.Location,
            warehouse.Capacity,
            warehouse.CreatedAt
        );
    }

    public WarehouseResponse? Update(int id, UpdateWarehouseRequest request)
    {
        var warehouse = _context.Warehouses.Find(id);
        if (warehouse == null) return null;

        if (request.WarehouseName != null) warehouse.WarehouseName = request.WarehouseName;
        if (request.Location != null) warehouse.Location = request.Location;
        if (request.Capacity.HasValue) warehouse.Capacity = request.Capacity;

        _context.SaveChanges();

        return new WarehouseResponse(
            warehouse.WarehouseId,
            warehouse.WarehouseName,
            warehouse.Location,
            warehouse.Capacity,
            warehouse.CreatedAt
        );
    }

    public bool Delete(int id)
    {
        var warehouse = _context.Warehouses.Find(id);
        if (warehouse == null) return false;

        _context.Warehouses.Remove(warehouse);
        _context.SaveChanges();
        return true;
    }
}
