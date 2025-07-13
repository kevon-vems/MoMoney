using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IExpenseService"/>.
/// </summary>
public class ExpenseService : IExpenseService
{
    private readonly RetirementPlannerContext _context;

    public ExpenseService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="Expense"/> records.
    /// </summary>
    public async Task<List<Expense>> GetAllAsync()
    {
        return await _context.Expenses.ToListAsync();
    }

    /// <summary>
    /// Get an <see cref="Expense"/> by id.
    /// </summary>
    public async Task<Expense?> GetByIdAsync(int id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    /// <summary>
    /// Add a new <see cref="Expense"/>.
    /// </summary>
    public async Task<Expense> AddAsync(Expense entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Expenses.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="Expense"/>.
    /// </summary>
    public async Task UpdateAsync(Expense entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Expenses.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="Expense"/> with the specified id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Expenses.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Expense {id} not found");
        }

        _context.Expenses.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
