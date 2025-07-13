using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="Income"/> entities.
/// </summary>
public interface IIncomeService
{
    /// <summary>
    /// Get all <see cref="Income"/> records.
    /// </summary>
    Task<List<Income>> GetAllAsync();

    /// <summary>
    /// Retrieve an <see cref="Income"/> by id.
    /// </summary>
    Task<Income?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="Income"/>.
    /// </summary>
    Task<Income> AddAsync(Income entity);

    /// <summary>
    /// Update an existing <see cref="Income"/>.
    /// </summary>
    Task UpdateAsync(Income entity);

    /// <summary>
    /// Delete the <see cref="Income"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
