using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="SurplusAllocationConfig"/> entities.
/// </summary>
public interface ISurplusAllocationService
{
    /// <summary>
    /// Get all <see cref="SurplusAllocationConfig"/> records.
    /// </summary>
    Task<List<SurplusAllocationConfig>> GetAllAsync();

    /// <summary>
    /// Retrieve a <see cref="SurplusAllocationConfig"/> by id.
    /// </summary>
    Task<SurplusAllocationConfig?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="SurplusAllocationConfig"/>.
    /// </summary>
    Task<SurplusAllocationConfig> AddAsync(SurplusAllocationConfig entity);

    /// <summary>
    /// Update an existing <see cref="SurplusAllocationConfig"/>.
    /// </summary>
    Task UpdateAsync(SurplusAllocationConfig entity);

    /// <summary>
    /// Delete the <see cref="SurplusAllocationConfig"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
