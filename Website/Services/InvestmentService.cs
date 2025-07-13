using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IInvestmentService"/>.
/// </summary>
public class InvestmentService : IInvestmentService
{
    private readonly RetirementPlannerContext _context;

    public InvestmentService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="Investment"/> records.
    /// </summary>
    public async Task<List<Investment>> GetAllAsync()
    {
        return await _context.Investments
            .Include(i => i.DistributionConfig)
            .ToListAsync();
    }

    /// <summary>
    /// Get an <see cref="Investment"/> by id.
    /// </summary>
    public async Task<Investment?> GetByIdAsync(int id)
    {
        return await _context.Investments
            .Include(i => i.DistributionConfig)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>
    /// Add a new <see cref="Investment"/>.
    /// </summary>
    public async Task<Investment> AddAsync(Investment entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Investments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="Investment"/>.
    /// </summary>
    public async Task UpdateAsync(Investment entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Investments.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="Investment"/> with the specified id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Investments.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Investment {id} not found");
        }

        _context.Investments.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
