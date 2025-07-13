using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="Asset"/> entities.
/// </summary>
public interface IAssetService
{
    /// <summary>
    /// Get all <see cref="Asset"/> records.
    /// </summary>
    Task<List<Asset>> GetAllAsync();

    /// <summary>
    /// Retrieve an <see cref="Asset"/> by id.
    /// </summary>
    Task<Asset?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="Asset"/>.
    /// </summary>
    Task<Asset> AddAsync(Asset entity);

    /// <summary>
    /// Update an existing <see cref="Asset"/>.
    /// </summary>
    Task UpdateAsync(Asset entity);

    /// <summary>
    /// Delete the <see cref="Asset"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
