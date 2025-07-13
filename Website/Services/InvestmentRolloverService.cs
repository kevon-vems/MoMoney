using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IInvestmentRolloverService"/>.
/// </summary>
public class InvestmentRolloverService : IInvestmentRolloverService
{
    private readonly RetirementPlannerContext _context;

    public InvestmentRolloverService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="InvestmentRollover"/> records.
    /// </summary>
    public async Task<List<InvestmentRollover>> GetAllAsync()
    {
        return await _context.InvestmentRollovers
            .Include(r => r.SourceInvestment)
            .Include(r => r.DestinationInvestment)
            .ToListAsync();
    }

    /// <summary>
    /// Get an <see cref="InvestmentRollover"/> by id.
    /// </summary>
    public async Task<InvestmentRollover?> GetByIdAsync(int id)
    {
        return await _context.InvestmentRollovers
            .Include(r => r.SourceInvestment)
            .Include(r => r.DestinationInvestment)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Add a new <see cref="InvestmentRollover"/>.
    /// </summary>
    public async Task<InvestmentRollover> AddAsync(InvestmentRollover entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.InvestmentRollovers.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="InvestmentRollover"/>.
    /// </summary>
    public async Task UpdateAsync(InvestmentRollover entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.InvestmentRollovers.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="InvestmentRollover"/> with the specified id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.InvestmentRollovers.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"InvestmentRollover {id} not found");
        }

        _context.InvestmentRollovers.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
