using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IIncomeService"/>.
/// </summary>
public class IncomeService : IIncomeService
{
    private readonly RetirementPlannerContext _context;

    public IncomeService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="Income"/> records.
    /// </summary>
    public async Task<List<Income>> GetAllAsync()
    {
        return await _context.Incomes.ToListAsync();
    }

    /// <summary>
    /// Get an <see cref="Income"/> by id.
    /// </summary>
    public async Task<Income?> GetByIdAsync(int id)
    {
        return await _context.Incomes.FindAsync(id);
    }

    /// <summary>
    /// Add a new <see cref="Income"/>.
    /// </summary>
    public async Task<Income> AddAsync(Income entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Incomes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="Income"/>.
    /// </summary>
    public async Task UpdateAsync(Income entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Incomes.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="Income"/> with the specified id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Incomes.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Income {id} not found");
        }

        _context.Incomes.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
