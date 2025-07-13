using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IAssetService"/>.
/// </summary>
public class AssetService : IAssetService
{
    private readonly RetirementPlannerContext _context;

    public AssetService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="Asset"/> records.
    /// </summary>
    public async Task<List<Asset>> GetAllAsync()
    {
        return await _context.Assets.ToListAsync();
    }

    /// <summary>
    /// Get an <see cref="Asset"/> by id.
    /// </summary>
    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _context.Assets.FindAsync(id);
    }

    /// <summary>
    /// Add a new <see cref="Asset"/>.
    /// </summary>
    public async Task<Asset> AddAsync(Asset entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Assets.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="Asset"/>.
    /// </summary>
    public async Task UpdateAsync(Asset entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Assets.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="Asset"/> with the specified id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Assets.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Asset {id} not found");
        }

        _context.Assets.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
