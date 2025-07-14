using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IScenarioService"/>.
/// </summary>
public class ScenarioService : IScenarioService
{
    private readonly RetirementPlannerContext _context;

    public ScenarioService(RetirementPlannerContext context)
    {
        _context = context;
    }

    public async Task<List<Scenario>> GetAllAsync()
    {
        return await _context.Scenarios.ToListAsync();
    }

    public async Task<Scenario?> GetByIdAsync(int id)
    {
        return await _context.Scenarios.FindAsync(id);
    }

    public async Task<Scenario> AddAsync(Scenario entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Scenarios.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Scenario entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.Scenarios.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Scenarios.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Scenario {id} not found");
        }

        _context.Scenarios.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task CloneInvestmentsAsync(int sourceScenarioId, int targetScenarioId)
    {
        if (sourceScenarioId == targetScenarioId) return;

        var sourceInvestments = await _context.Investments
            .Include(i => i.DistributionConfig)
            .Where(i => i.ScenarioId == sourceScenarioId)
            .ToListAsync();

        var existingNames = await _context.Investments
            .Where(i => i.ScenarioId == targetScenarioId)
            .Select(i => i.Name)
            .ToListAsync();

        foreach (var inv in sourceInvestments)
        {
            var newName = GenerateUniqueName(inv.Name, existingNames);
            existingNames.Add(newName);

            var clone = new Investment
            {
                Name = newName,
                ScenarioId = targetScenarioId,
                InvestmentType = inv.InvestmentType,
                CurrentBalance = inv.CurrentBalance,
                ExpectedDividendYield = inv.ExpectedDividendYield,
                ExpectedGrowthRate = inv.ExpectedGrowthRate,
                ExpectedReturnOfCapitalRate = inv.ExpectedReturnOfCapitalRate,
                ExpenseRatio = inv.ExpenseRatio,
                ExpectedTurnoverRate = inv.ExpectedTurnoverRate,
                IsFederalTaxExempt = inv.IsFederalTaxExempt,
                IsStateTaxExempt = inv.IsStateTaxExempt,
                WithdrawalPriority = inv.WithdrawalPriority,
                RequiresRMD = inv.RequiresRMD,
                RMDStartAge = inv.RMDStartAge,
                PersonId = inv.PersonId,
                DistributionConfig = inv.DistributionConfig
                    .Select(d => new InvestmentDistributionConfig
                    {
                        Category = d.Category,
                        AnnualPercentage = d.AnnualPercentage
                    }).ToList()
            };

            _context.Investments.Add(clone);
        }

        await _context.SaveChangesAsync();
    }

    private static string GenerateUniqueName(string baseName, ICollection<string> existingNames)
    {
        if (!existingNames.Contains(baseName))
            return baseName;

        var counter = 1;
        var newName = $"{baseName} ({counter})";
        while (existingNames.Contains(newName))
        {
            counter++;
            newName = $"{baseName} ({counter})";
        }
        return newName;
    }
}
