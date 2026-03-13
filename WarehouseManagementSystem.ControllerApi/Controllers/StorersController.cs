using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.ControllerApi.DTOs;
using PickAPile.Helpers;

namespace WarehouseManagementSystem.ControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorersController : ControllerBase
{
    private readonly AppDbContext _context;

    public StorersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var storers = _context.Storers
            .Select(s => new StorerResponse(
                s.StorerId,
                s.FullName,
                s.Email,
                s.Phone,
                s.Address,
                s.CreatedAt
            ))
            .ToList();

        return Common.Success(storers, 200, "Storers retrieved successfully");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var storer = _context.Storers.Find(id);
        if (storer == null)
            return Common.Error(404, "Storer not found");

        var response = new StorerResponse(
            storer.StorerId,
            storer.FullName,
            storer.Email,
            storer.Phone,
            storer.Address,
            storer.CreatedAt
        );

        return Common.Success(response, 200, "Storer retrieved successfully");
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStorerRequest request)
    {
        var storer = new Storer
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            CreatedAt = DateTime.Now
        };

        _context.Storers.Add(storer);
        _context.SaveChanges();

        var response = new StorerResponse(
            storer.StorerId,
            storer.FullName,
            storer.Email,
            storer.Phone,
            storer.Address,
            storer.CreatedAt
        );

        return Common.Success(response, 201, "Storer created successfully");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateStorerRequest request)
    {
        var storer = _context.Storers.Find(id);
        if (storer == null)
            return Common.Error(404, "Storer not found");

        if (request.FullName != null) storer.FullName = request.FullName;
        if (request.Email != null) storer.Email = request.Email;
        if (request.Phone != null) storer.Phone = request.Phone;
        if (request.Address != null) storer.Address = request.Address;

        _context.SaveChanges();

        var response = new StorerResponse(
            storer.StorerId,
            storer.FullName,
            storer.Email,
            storer.Phone,
            storer.Address,
            storer.CreatedAt
        );

        return Common.Success(response, 200, "Storer updated successfully");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var storer = _context.Storers.Find(id);
        if (storer == null)
            return Common.Error(404, "Storer not found");

        _context.Storers.Remove(storer);
        _context.SaveChanges();

        return Common.Success(null, 204, "Storer deleted successfully");
    }
}
