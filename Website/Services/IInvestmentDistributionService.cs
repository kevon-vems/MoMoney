using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for <see cref="InvestmentDistributionConfig"/> entities.
/// </summary>
public interface IInvestmentDistributionService
{
    /// <summary>
    /// Get all <see cref="InvestmentDistributionConfig"/> records.
    /// </summary>
    Task<List<InvestmentDistributionConfig>> GetAllAsync();

    /// <summary>
    /// Retrieve an <see cref="InvestmentDistributionConfig"/> by id.
    /// </summary>
    Task<InvestmentDistributionConfig?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new <see cref="InvestmentDistributionConfig"/>.
    /// </summary>
    Task<InvestmentDistributionConfig> AddAsync(InvestmentDistributionConfig entity);

    /// <summary>
    /// Update an existing <see cref="InvestmentDistributionConfig"/>.
    /// </summary>
    Task UpdateAsync(InvestmentDistributionConfig entity);

    /// <summary>
    /// Delete the <see cref="InvestmentDistributionConfig"/> with the specified id.
    /// </summary>
    Task DeleteAsync(int id);
}
