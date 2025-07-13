using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="Person"/> entities.
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Get all <see cref="Person"/> records.
    /// </summary>
    Task<List<Person>> GetAllAsync();

    /// <summary>
    /// Retrieve a <see cref="Person"/> by id.
    /// </summary>
    Task<Person?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="Person"/>.
    /// </summary>
    Task<Person> AddAsync(Person entity);

    /// <summary>
    /// Update an existing <see cref="Person"/>.
    /// </summary>
    Task UpdateAsync(Person entity);

    /// <summary>
    /// Delete the <see cref="Person"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
