using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// Provides CRUD operations for scenarios and helper methods for cloning data between scenarios.
/// </summary>
public interface IScenarioService
{
    Task<List<Scenario>> GetAllAsync();
    Task<Scenario?> GetByIdAsync(int id);
    Task<Scenario> AddAsync(Scenario entity);
    Task UpdateAsync(Scenario entity);
    Task DeleteAsync(int id);

    /// <summary>
    /// Clone all investments from the source scenario to the target scenario.
    /// </summary>
    Task CloneInvestmentsAsync(int sourceScenarioId, int targetScenarioId);
}
