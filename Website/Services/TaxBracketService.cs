using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="ITaxBracketService"/>.
/// </summary>
public class TaxBracketService : ITaxBracketService
{
    private readonly RetirementPlannerContext _context;

    public TaxBracketService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="TaxBracket"/> records.
    /// </summary>
    public async Task<List<TaxBracket>> GetAllAsync()
    {
        return await _context.TaxBrackets.ToListAsync();
    }

    /// <summary>
    /// Get a <see cref="TaxBracket"/> by id.
    /// </summary>
    public async Task<TaxBracket?> GetByIdAsync(int id)
    {
        return await _context.TaxBrackets.FindAsync(id);
    }

    /// <summary>
    /// Add a new <see cref="TaxBracket"/>.
    /// </summary>
    public async Task<TaxBracket> AddAsync(TaxBracket entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.TaxBrackets.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="TaxBracket"/>.
    /// </summary>
    public async Task UpdateAsync(TaxBracket entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.TaxBrackets.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="TaxBracket"/> with the given id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.TaxBrackets.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"TaxBracket {id} not found");
        }

        _context.TaxBrackets.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
