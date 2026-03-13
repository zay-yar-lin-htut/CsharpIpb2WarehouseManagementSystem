using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Database.AppDbContextModels;
using WarehouseManagementSystem.MinimalApi.DTOs;

namespace WarehouseManagementSystem.MinimalApi.Services;

public class StorerService
{
    private readonly AppDbContext _context;

    public StorerService(AppDbContext context)
    {
        _context = context;
    }

    public List<StorerResponse> GetAll()
    {
        return _context.Storers
            .Select(s => new StorerResponse(
                s.StorerId,
                s.FullName,
                s.Email,
                s.Phone,
                s.Address,
                s.CreatedAt
            ))
            .ToList();
    }

    public StorerResponse? GetById(int id)
    {
        var storer = _context.Storers.Find(id);
        if (storer == null) return null;

        return new StorerResponse(
            storer.StorerId,
            storer.FullName,
            storer.Email,
            storer.Phone,
            storer.Address,
            storer.CreatedAt
        );
    }

    public StorerResponse Create(CreateStorerRequest request)
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

        return new StorerResponse(
            storer.StorerId,
            storer.FullName,
            storer.Email,
            storer.Phone,
            storer.Address,
            storer.CreatedAt
        );
    }

    public StorerResponse? Update(int id, UpdateStorerRequest request)
    {
        var storer = _context.Storers.Find(id);
        if (storer == null) return null;

        if (request.FullName != null) storer.FullName = request.FullName;
        if (request.Email != null) storer.Email = request.Email;
        if (request.Phone != null) storer.Phone = request.Phone;
        if (request.Address != null) storer.Address = request.Address;

        _context.SaveChanges();

        return new StorerResponse(
            storer.StorerId,
            storer.FullName,
            storer.Email,
            storer.Phone,
            storer.Address,
            storer.CreatedAt
        );
    }

    public bool Delete(int id)
    {
        var storer = _context.Storers.Find(id);
        if (storer == null) return false;

        _context.Storers.Remove(storer);
        _context.SaveChanges();
        return true;
    }
}
