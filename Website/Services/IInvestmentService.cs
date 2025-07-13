using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="Investment"/> entities.
/// </summary>
public interface IInvestmentService
{
    /// <summary>
    /// Get all <see cref="Investment"/> records.
    /// </summary>
    Task<List<Investment>> GetAllAsync();

    /// <summary>
    /// Retrieve an <see cref="Investment"/> by id.
    /// </summary>
    Task<Investment?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="Investment"/>.
    /// </summary>
    Task<Investment> AddAsync(Investment entity);

    /// <summary>
    /// Update an existing <see cref="Investment"/>.
    /// </summary>
    Task UpdateAsync(Investment entity);

    /// <summary>
    /// Delete the <see cref="Investment"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
