using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IInvestmentDistributionService"/>.
/// </summary>
public class InvestmentDistributionService : IInvestmentDistributionService
{
    private readonly RetirementPlannerContext _context;

    public InvestmentDistributionService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="InvestmentDistributionConfig"/> records.
    /// </summary>
    public async Task<List<InvestmentDistributionConfig>> GetAllAsync()
    {
        return await _context.InvestmentDistributionConfigs
            .Include(d => d.Investment)
            .ToListAsync();
    }

    /// <summary>
    /// Get an <see cref="InvestmentDistributionConfig"/> by id.
    /// </summary>
    public async Task<InvestmentDistributionConfig?> GetByIdAsync(int id)
    {
        return await _context.InvestmentDistributionConfigs
            .Include(d => d.Investment)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    /// <summary>
    /// Add a new <see cref="InvestmentDistributionConfig"/>.
    /// </summary>
    public async Task<InvestmentDistributionConfig> AddAsync(InvestmentDistributionConfig entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.InvestmentDistributionConfigs.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="InvestmentDistributionConfig"/>.
    /// </summary>
    public async Task UpdateAsync(InvestmentDistributionConfig entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.InvestmentDistributionConfigs.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="InvestmentDistributionConfig"/> with the specified id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.InvestmentDistributionConfigs.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"InvestmentDistributionConfig {id} not found");
        }

        _context.InvestmentDistributionConfigs.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
