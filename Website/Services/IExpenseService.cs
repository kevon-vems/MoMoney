using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="Expense"/> entities.
/// </summary>
public interface IExpenseService
{
    /// <summary>
    /// Get all <see cref="Expense"/> records.
    /// </summary>
    Task<List<Expense>> GetAllAsync();

    /// <summary>
    /// Retrieve an <see cref="Expense"/> by id.
    /// </summary>
    Task<Expense?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="Expense"/>.
    /// </summary>
    Task<Expense> AddAsync(Expense entity);

    /// <summary>
    /// Update an existing <see cref="Expense"/>.
    /// </summary>
    Task UpdateAsync(Expense entity);

    /// <summary>
    /// Delete the <see cref="Expense"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
