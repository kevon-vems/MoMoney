using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

/// <summary>
/// Represents a complete set of planning parameters for a single simulation scenario.
/// </summary>
public class Scenario
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public FilingStatus FilingStatus { get; set; } = FilingStatus.Single;

    [StringLength(100)]
    public string State { get; set; } = string.Empty;

    public int SimulationStartYear { get; set; } = DateTime.UtcNow.Year;

    public int SimulationEndYear { get; set; } = DateTime.UtcNow.Year + 30;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GeneralInflationRate { get; set; } = 0.02m;

    [Column(TypeName = "decimal(18,2)")]
    public decimal HealthcareInflationRate { get; set; } = 0.04m;

    public virtual ICollection<Investment> Investments { get; set; } = new List<Investment>();
    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    public virtual ICollection<TaxBracket> TaxBrackets { get; set; } = new List<TaxBracket>();
    public virtual ICollection<InvestmentRollover> InvestmentRollovers { get; set; } = new List<InvestmentRollover>();
    public virtual ICollection<SurplusAllocationConfig> SurplusAllocations { get; set; } = new List<SurplusAllocationConfig>();
}
