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
    [HttpGet]
    public IActionResult GetAll()
    {
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var storers = context.Storers
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
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var storer = context.Storers.Find(id);
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
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailExists = context.Storers.Any(s => s.Email == request.Email);
            if (emailExists)
                return Common.Error(400, "Email already exists");
        }

        var storer = new Storer
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            CreatedAt = DateTime.Now
        };

        context.Storers.Add(storer);
        context.SaveChanges();

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
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var storer = context.Storers.Find(id);
        if (storer == null)
            return Common.Error(404, "Storer not found");

        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailExists = context.Storers.Any(s => s.Email == request.Email && s.StorerId != id);
            if (emailExists)
                return Common.Error(400, "Email already exists");
        }

        if (request.FullName != null) storer.FullName = request.FullName;
        if (request.Email != null) storer.Email = request.Email;
        if (request.Phone != null) storer.Phone = request.Phone;
        if (request.Address != null) storer.Address = request.Address;

        context.SaveChanges();

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
        var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        
        var storer = context.Storers.Find(id);
        if (storer == null)
            return Common.Error(404, "Storer not found");

        context.Storers.Remove(storer);
        context.SaveChanges();

        return Common.Success(null, 204, "Storer deleted successfully");
    }
}
