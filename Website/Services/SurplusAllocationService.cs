using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="ISurplusAllocationService"/>.
/// </summary>
public class SurplusAllocationService : ISurplusAllocationService
{
    private readonly RetirementPlannerContext _context;

    public SurplusAllocationService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="SurplusAllocationConfig"/> records.
    /// </summary>
    public async Task<List<SurplusAllocationConfig>> GetAllAsync()
    {
        return await _context.SurplusAllocations
            .Include(s => s.Investment)
            .ToListAsync();
    }

    /// <summary>
    /// Get a <see cref="SurplusAllocationConfig"/> by id.
    /// </summary>
    public async Task<SurplusAllocationConfig?> GetByIdAsync(int id)
    {
        return await _context.SurplusAllocations
            .Include(s => s.Investment)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Add a new <see cref="SurplusAllocationConfig"/>.
    /// </summary>
    public async Task<SurplusAllocationConfig> AddAsync(SurplusAllocationConfig entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.SurplusAllocations.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="SurplusAllocationConfig"/>.
    /// </summary>
    public async Task UpdateAsync(SurplusAllocationConfig entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.SurplusAllocations.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="SurplusAllocationConfig"/> with the given id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.SurplusAllocations.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"SurplusAllocationConfig {id} not found");
        }

        _context.SurplusAllocations.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
