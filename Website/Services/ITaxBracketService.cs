using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="TaxBracket"/> entities.
/// </summary>
public interface ITaxBracketService
{
    /// <summary>
    /// Get all <see cref="TaxBracket"/> records.
    /// </summary>
    Task<List<TaxBracket>> GetAllAsync();

    /// <summary>
    /// Retrieve a <see cref="TaxBracket"/> by id.
    /// </summary>
    Task<TaxBracket?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="TaxBracket"/>.
    /// </summary>
    Task<TaxBracket> AddAsync(TaxBracket entity);

    /// <summary>
    /// Update an existing <see cref="TaxBracket"/>.
    /// </summary>
    Task UpdateAsync(TaxBracket entity);

    /// <summary>
    /// Delete the <see cref="TaxBracket"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
