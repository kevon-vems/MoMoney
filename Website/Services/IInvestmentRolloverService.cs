using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="InvestmentRollover"/> entities.
/// </summary>
public interface IInvestmentRolloverService
{
    /// <summary>
    /// Get all <see cref="InvestmentRollover"/> records.
    /// </summary>
    Task<List<InvestmentRollover>> GetAllAsync();

    /// <summary>
    /// Retrieve an <see cref="InvestmentRollover"/> by id.
    /// </summary>
    Task<InvestmentRollover?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="InvestmentRollover"/>.
    /// </summary>
    Task<InvestmentRollover> AddAsync(InvestmentRollover entity);

    /// <summary>
    /// Update an existing <see cref="InvestmentRollover"/>.
    /// </summary>
    Task UpdateAsync(InvestmentRollover entity);

    /// <summary>
    /// Delete the <see cref="InvestmentRollover"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
